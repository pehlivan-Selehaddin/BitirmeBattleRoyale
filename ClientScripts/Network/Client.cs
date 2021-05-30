using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;

    public string ip = "192.168.1.8";
    public int port = 6060;
    public int myId = 0;
    public TCP tcp;
    public UDP udp;

    public bool isConnected = false;

    private delegate void PacketHandler(Packet packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Client already exixst ");
            Destroy(this);
        }
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public string ConnectToServer()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return "Please check your internet connection";//lütfen internetinizi kontrol edin
        }
        tcp = new TCP();
        udp = new UDP();

        InitializeClientData();

        isConnected = true;
        tcp.Connect();
        //udp.Connect(port);
        if (tcp.socket.Client.Connected)
        {
            return string.Empty;
        }
        else
        {
            return "Server şuanda bakımda lütfen daha sonra tekrar deneyin..";
        }
    }

    public class TCP
    {
        public TcpClient socket;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void Connect()
        {
            socket = new TcpClient()
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];

            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
            {
                return;
            }


            stream = socket.GetStream();

            receivedData = new Packet();


            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Error sending data {ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {

                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    instance.Disconnect();//client disconnect
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                Disconnect();//tcp disconnect
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {

                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {

                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {

                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        // Debug.LogError("Packet Id ==> " + _packetId);
                        packetHandlers[_packetId](_packet);
                    }
                });

                _packetLength = 0; // Reset packet length
                if (receivedData.UnreadLength() >= 4)
                {

                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {

                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        private void Disconnect()
        {
            instance.Disconnect();


            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    public class UDP
    {
        public UdpClient socket;
        IPEndPoint endPoint;

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
        }

        public void Connect(int _localPort)
        {
            socket = new UdpClient(_localPort);

            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallback, null);

            using (Packet _packet = new Packet())
            {
                SendData(_packet);
            }

        }
        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(instance.myId);
                if (socket != null)
                {
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {

                Debug.Log($"Error sending data to server via UDP {ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                if (data.Length < 4)
                {
                    instance.Disconnect();//client disconnect
                    return;
                }
                HandleData(data);
            }
            catch (Exception)
            {

                Disconnect();//UDP DISCONNECT
            }
        }
        private void HandleData(byte[] _data)
        {
            using (Packet _packet = new Packet(_data))
            {
                int packetLength = _packet.ReadInt();
                _data = _packet.ReadBytes(packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_data))
                {
                    int packetID = _packet.ReadInt();
                    packetHandlers[packetID](_packet);
                }

            });
        }
        private void Disconnect()
        {
            instance.Disconnect();

            endPoint = null;
            socket = null;
        }
    }
    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int)ServerPackets.Welcome ,ClientHandle.Welcome},
            {(int)ServerPackets.RegisterUserInfoReceived ,ClientHandle.RegisterUserInfoReceived},
            {(int)ServerPackets.LoginUserInfoReceived ,ClientHandle.LoginUserInfoReceived},
            {(int)ServerPackets.StartGame ,ClientHandle.StartGame},
            {(int)ServerPackets.SpawnUser ,ClientHandle.SpawnUser},
            {(int)ServerPackets.PlayerAnimationReceived ,ClientHandle.PlayerAnimationReceived},
            {(int)ServerPackets.PlayerPosition ,ClientHandle.PlayerPosition},
            {(int)ServerPackets.PlayerRotation ,ClientHandle.PlayerRotation},
            {(int)ServerPackets.TreeHealth ,ClientHandle.TreeHealth},
             {(int)ServerPackets.SpawnResourcesReceived ,ClientHandle.SpawnResourcesReceived},
             {(int)ServerPackets.PickUpResourceReceived ,ClientHandle.PickUpResourceReceived},
              {(int)ServerPackets.SpawnSheep ,ClientHandle.SpawnSheep},
              {(int)ServerPackets.SheepPosition ,ClientHandle.SheepPosition},
              {(int)ServerPackets.SheepAnimation,ClientHandle.SheepAnimation},
              {(int)ServerPackets.SpawnStone,ClientHandle.SpawnStone},
              {(int)ServerPackets.StoneHealth,ClientHandle.StoneHealth},
              {(int)ServerPackets.SpawnIron,ClientHandle.SpawnIron},
              {(int)ServerPackets.IronHealth,ClientHandle.IronHealth},
        };
        Debug.Log("Initialized client data");
    }
    public void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            tcp.socket.Close();
            udp.socket.Close();

            Debug.Log($"Disconnected from the server.");
        }
    }

}
