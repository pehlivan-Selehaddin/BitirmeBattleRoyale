﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server
{
    public static int MaxPlayers { get; private set; }

    public static int Port { get; private set; }
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

    public delegate void PacketHandler(int _fromClient, Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;



    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static void Start(int _maxPlayers, int _port)
    {
        MaxPlayers = _maxPlayers;
        Port = _port;

        Debug.Log("Server Initializing ... ");

        InitializeServerData();

        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);

        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        Debug.Log($"Server started on {Port}");
    }

    private static void TCPConnectCallBack(IAsyncResult _result)
    {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);

        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);

        Debug.Log($"Bir oyuncu {_client.Client.RemoteEndPoint}  ip ile sunucuya bağlandı...");

        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (clients[i].tcp.socket == null)
            {
                clients[i].tcp.Connect(_client);// sunucuya bağladık.
                return;
            }
        }
        Debug.Log($"{_client.Client.RemoteEndPoint} Servera bağlanma başarısız : Server dolu ..!");
    }
    private static void UDPReceiveCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);

            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (_data.Length < 4)
            {
                return;
            }

            using (Packet _packet = new Packet(_data))
            {
                int _clientId = _packet.ReadInt();
                if (_clientId == 0)
                {
                    return;
                }
                if (clients[_clientId].udp.endPoint == null)
                {
                    clients[_clientId].udp.Connect(_clientEndPoint);
                    return;
                }

                if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                {
                    clients[_clientId].udp.HandleData(_packet);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error receiving data {ex}");
        }
    }
    public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
    {
        try
        {
            if (_clientEndPoint != null)
            {
                udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error sending data to {_clientEndPoint}  via UDP:  {ex}");
        }
    }
    public static void InitializeServerData()
    {
        for (int i = 1; i <= MaxPlayers; i++)
        {
            clients.Add(i, new Client(i));
        }
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int)ClientPackets.WelcomeReceived,ServerHandle.WelcomeReceived },
            {(int)ClientPackets.RegisterUserInfo,ServerHandle.RegisterUserInfo},
            {(int)ClientPackets.LoginUserInfo,ServerHandle.LoginUserInfo},
            {(int)ClientPackets.FindMatch,ServerHandle.FindMatch},
            {(int)ClientPackets.JoinedGame,ServerHandle.JoinedGame},
            {(int)ClientPackets.PlayerMoveDirection,ServerHandle.PlayerMoveDirection},
            {(int)ClientPackets.PlayerAnimation,ServerHandle.PlayerAnimation},
            {(int)ClientPackets.PlayerInAim,ServerHandle.PlayerInAim},
            {(int)ClientPackets.HitTree,ServerHandle.HitTree},
            {(int)ClientPackets.SpawnResources,ServerHandle.SpawnResources},
             {(int)ClientPackets.PickupResource,ServerHandle.PickupResource},
             {(int)ClientPackets.HitSheep,ServerHandle.HitSheep},
             {(int)ClientPackets.HitStone,ServerHandle.HitStone},
             {(int)ClientPackets.HitIron,ServerHandle.HitIron},

        };//initializing packets..

        Debug.Log("Paketler baslatiliyor...");
    }

    public static void Stop()
    {
        tcpListener.Stop();
        udpListener.Close();
    }
}
