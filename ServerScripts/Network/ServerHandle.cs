using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}  username  : {username}");

        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{username}\"  (ID  : {_fromClient}) has assumed the wrong client ID   ({_clientIdCheck}) !  ");
        }

        //TODO : send player into the game
        //  Server.clients[_fromClient].SendIntoGame(username);
    }
    public static void RegisterUserInfo(int _fromClient, Packet _packet)
    {
        string username = _packet.ReadString();
        string password = _packet.ReadString();

        NetworkManager.instance.CallRegister(_fromClient, username, password);
    }

    public static void LoginUserInfo(int _fromClient, Packet _packet)
    {
        string username = _packet.ReadString();
        string password = _packet.ReadString();


        NetworkManager.instance.CallLogin(_fromClient, username, password);
    }

    public static void FindMatch(int _fromClient, Packet _packet)
    {
        MatchMaker.instance.FindMatch(_fromClient);
    }
    public static void JoinedGame(int _fromClient, Packet _packet)
    {
        Server.clients[_fromClient].isInGame = true;
        Match userMatch = Server.clients[_fromClient].match;

        if (userMatch == null) return;

        userMatch.inGamePlayers.Add(_fromClient);

        Server.clients[_fromClient].player = NetworkManager.instance.IntantiatePlayer(_fromClient);//Find match yapanlar için Player nesnesi oluşturuldu (SPAWN USER)
        Server.clients[_fromClient].player.Initialize(_fromClient);


        for (int i = 0; i < userMatch.clients.Count; i++)
        {
            if (userMatch.clients[i].isInGame)//sonradan oyuna girene oyundıkileri gönderemiyoz.
            {
                ServerSend.SpawnUser(_fromClient, userMatch.clients[i].id, userMatch.clients[i].player.startPos, userMatch.clients[i].player.startRot, userMatch.id);//kendime diğer kullanıcıları gönderdim.(kendimde dahil)
                if (userMatch.clients[i].id != _fromClient)
                    ServerSend.SpawnUser(userMatch.clients[i].id, _fromClient, Server.clients[_fromClient].player.startPos, Server.clients[_fromClient].player.startRot, userMatch.id);//diğer kullanıcılara kendimi gönderdim
            }
        }
    }
    public static void PlayerMoveDirection(int _fromClient, Packet _packet)
    {
        Vector3 moveDirection = _packet.ReadVector3();

        Server.clients[_fromClient].player.Move(moveDirection);
    }

    public static void PlayerAnimation(int _fromClient, Packet _packet)
    {
        string animName = _packet.ReadString();
        AnimType animType = (AnimType)_packet.ReadInt();
        bool isPlay = _packet.ReadBool();
        float value = _packet.ReadFloat();

        ServerSend.PlayerAnimationReceived(_fromClient, animName, animType, isPlay, value);
    }

    public static void PlayerInAim(int _fromClient, Packet _packet)
    {
        Vector3 direction = _packet.ReadVector3();
        int aimType = _packet.ReadInt();
        bool isMoving = _packet.ReadBool();
        Vector3 moveDirection = _packet.ReadVector3();

        Server.clients[_fromClient].player.PlayerInAim(direction,(AimType)aimType,isMoving,moveDirection);
    }

    public static void HitTree(int _fromClient, Packet _packet)
    {
        int damage=_packet.ReadInt();
        string treeId = _packet.ReadString();

        NetworkManager.instance.HitTree(_fromClient,treeId, damage); 
    }
    public static void HitStone(int _fromClient, Packet _packet)
    {
        int damage = _packet.ReadInt();
        string stoneId = _packet.ReadString();

        NetworkManager.instance.HitStone(_fromClient, stoneId, damage);
    }
    public static void HitIron(int _fromClient, Packet _packet)
    {
        int damage = _packet.ReadInt();
        string ironId = _packet.ReadString();

        NetworkManager.instance.HitIron(_fromClient, ironId, damage);
    }
    public static void SpawnResources(int _fromClient, Packet _packet)
    {
        int resourcesType = _packet.ReadInt();
        int amount = _packet.ReadInt();
        Vector3 spawnPos= _packet.ReadVector3();

        ServerSend.SpawnResourcesReceived(_fromClient,resourcesType,amount,spawnPos);
    }

    public static void PickupResource(int _fromClient, Packet _packet)
    {
        string resourceId = _packet.ReadString();

        ServerSend.PickupResourceReceived(_fromClient, resourceId);
    }

    public static void HitSheep(int _fromClient, Packet _packet)
    {
        int damage = _packet.ReadInt();
        string sheepId = _packet.ReadString();

        NetworkManager.instance.HitSheep(sheepId,damage);
    }
}
public enum AnimType
{
    floatAnim = 0,
    boolAnim = 1,
    triggerAnim = 2,
}
public enum AimType
{
    BowAim = 0,
    PistolAim = 1,
}

