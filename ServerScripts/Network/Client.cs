using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;


public class Client
{
    public static int dataBufferSize = 4096;

    public int id;
    public int DBid;
    public Player player;
    public Match match;


    public bool isInGame = false;

    public Vector3 moveDirection;

    public TCP tcp;
    public UDP udp;

    public Client(int _clientId)
    {
        id = _clientId;
        tcp = new TCP(id);
        udp = new UDP(id);
    }

    public class TCP
    {
        public TcpClient socket;
        private readonly int id;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public TCP(int _id)
        {
            id = _id;
        }
        public void Connect(TcpClient _socket)
        {
            socket = _socket;

            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream();

            receivedData = new Packet();
            receiveBuffer = new byte[dataBufferSize];

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            ServerSend.Welcome(id, "Welcome to the Server");
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int byteLength = stream.EndRead(_result);
                if (byteLength <= 0)
                {
                    Server.clients[id].Disconnect();//disconnect//this Client disconnect
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(receiveBuffer, data, byteLength);

                receivedData.Reset(HandleData(data));

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            }
            catch (Exception ex)
            {
                Debug.Log($"Receiving Error {ex}");
                Server.clients[id].Disconnect();
            }
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
                Debug.Log($"Error sending to data player {id}  error : {ex}");
                Server.clients[id].Disconnect();
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
                        Server.packetHandlers[_packetId](id, _packet);
                    }
                });

                _packetLength = 0;
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
        public void Disconnect()
        {
            socket.Close();
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }
    public class UDP
    {
        public IPEndPoint endPoint;

        private int id;

        public UDP(int _id)
        {
            id = _id;
        }
        public void Connect(IPEndPoint _endPoint)
        {
            endPoint = _endPoint;
        }
        public void SendData(Packet _packet)
        {
            Server.SendUDPData(endPoint, _packet);
        }
        public void HandleData(Packet _packet)
        {
            int packetLength = _packet.ReadInt();
            byte[] _packetBytes = _packet.ReadBytes(packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(_packetBytes))
                {
                    int packetId = packet.ReadInt();
                    Server.packetHandlers[packetId](id, packet);
                }
            });
        }
        public void Disconnect()
        {
            endPoint = null;
        }
    }
    public void Disconnect()
    {
        Debug.Log($"{tcp.socket.Client.RemoteEndPoint}  has disconnected");
        //Main gameObjec gibi kavramlar main threadda calısmalı yoksa server patlıyor

        ThreadManager.ExecuteOnMainThread(() =>
        {
            DeleteUserInMatch();
        });

        tcp.Disconnect();
        udp.Disconnect();

        //ServerSend.PlayerDisconnected(id);
    }

    private void DeleteUserInMatch()
    {
        Server.clients[id].match.inGamePlayers.Remove(id);
        for (int i = 0; i < MatchMaker.instance.matches.Length; i++)
        {
            foreach (var client in MatchMaker.instance.matches[i].clients)
            {
                if (client == this)
                {
                    if (Server.clients[client.id].player!=null)
                    {
                        NetworkManager.instance.DestroyPlayer(id);
                    }
                    MatchMaker.instance.matches[i].clients.Remove(client);
                    if (MatchMaker.instance.matches[i].clients.Count<=0)
                    {
                        NetworkManager.instance.ClearSheepData(Server.clients[client.id].match.id);/*SHEEEP*/
                        NetworkManager.instance.ClearStoneData(Server.clients[client.id].match.id);
                        NetworkManager.instance.ClearIronData(Server.clients[client.id].match.id);
                        MatchMaker.instance.matches[i].isStart = false;///**************** tekrar bak
                    }
                    return;
                }
            }
        }
        
    }
}
