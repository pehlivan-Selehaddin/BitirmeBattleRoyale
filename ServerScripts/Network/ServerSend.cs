using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();

        Server.clients[_toClient].tcp.SendData(_packet);

    }

    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }


    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }

    private static void SendUDPDataToAllMatch(Match match, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i < match.clients.Count; i++)
        {
            match.clients[i].udp.SendData(_packet);
        }
    }

    private static void SendUDPDataToAllMatchInGame(Match match, Packet _packet)//spawn olmuş kişilere gönderilecek
    {
        _packet.WriteLength();
        for (int i = 0; i < match.inGamePlayers.Count; i++)
        {
            Server.clients[match.inGamePlayers[i]].udp.SendData(_packet);
        }
    }

  

    private static void SendTCPDataToAllMatch(Match match, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i < match.clients.Count; i++)
        {
            match.clients[i].tcp.SendData(_packet);
        }
    }
    private static void SendTCPDataToAllMatchInGame(Match match, Packet _packet)//spawn olmuş kişilere gönderilecek
    {

        _packet.WriteLength();
        for (int i = 0; i < match.inGamePlayers.Count; i++)
        {
            Server.clients[match.inGamePlayers[i]].tcp.SendData(_packet);
        }
    }


    private static void SendTCPDataToAllMatch(Match match, int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i < match.clients.Count; i++)
        {
            if (match.clients[i].id == _exceptClient) continue;

            match.clients[i].tcp.SendData(_packet);
        }
    }


    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.Welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);


            SendTCPData(_toClient, _packet);
        }
    }

    public static void RegisterUserInfoReceived(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.RegisterUserInfoReceived))
        {
            _packet.Write(_msg);


            SendTCPData(_toClient, _packet);
        }
    }

    

    public static void LoginUserInfoReceived(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.LoginUserInfoReceived))
        {
            _packet.Write(_msg);

            SendTCPData(_toClient, _packet);
        }
    }
    public static void StartGame(Match match)
    {
        using (Packet _packet = new Packet((int)ServerPackets.StartGame))
        {
            SendTCPDataToAllMatch(match, _packet);
        }
    }

   

    public static void SpawnUser(int toId, int id, Vector3 spawnPos, Quaternion spawnRot, int matchId)
    {
        Debug.Log("FromclientId   ==>  " + toId + "    userMatchclient id ==>  " + id);
        using (Packet _packet = new Packet((int)ServerPackets.SpawnUser))
        {
            _packet.Write(id);
            _packet.Write(0);//farklı karakterler gelirse değişecek
            _packet.Write(spawnPos);
            _packet.Write(spawnRot);
            _packet.Write(matchId);

            SendTCPData(toId, _packet);
        }
    }
    public static void PlayerPosition(Match match, int id, Vector3 position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.PlayerPosition))
        {
            _packet.Write(id);
            _packet.Write(position);

            SendUDPDataToAllMatchInGame(match, _packet);
        }
    }


    public static void PlayerRotation(Match match, int id, Quaternion rotation)
    {
        using (Packet _packet = new Packet((int)ServerPackets.PlayerRotation))
        {
            _packet.Write(id);
            _packet.Write(rotation);

            SendUDPDataToAllMatchInGame(match, _packet);
        }
    }

   

    public static void PlayerAnimationReceived(int id, string animName, AnimType animType, bool isPlay, float value)
    {
        using (Packet _packet = new Packet((int)ServerPackets.PlayerAnimationReceived))
        {
            _packet.Write(id);
            _packet.Write(animName);
            _packet.Write((int)animType);
            _packet.Write(isPlay);
            _packet.Write(value);

            SendTCPDataToAllMatchInGame(Server.clients[id].match, _packet);
        }
    }
    public static void TreeHealth(int id, string treeId, int health)
    {
        using (Packet _packet = new Packet((int)ServerPackets.TreeHealth))
        {
            _packet.Write(treeId);
            _packet.Write(health);

            Match match = Server.clients[id].match;

            SendTCPDataToAllMatchInGame(match, _packet);
        }
    }

    public static void StoneHealth(int id, string stoneId, int health)
    {
        using (Packet _packet = new Packet((int)ServerPackets.StoneHealth))
        {
            _packet.Write(stoneId);
            _packet.Write(health);

            Match match = Server.clients[id].match;

            SendTCPDataToAllMatchInGame(match, _packet);
        }
    }

    public static void IronHealth(int id, string ironId, int health)
    {
        using (Packet _packet = new Packet((int)ServerPackets.IronHealth))
        {
            _packet.Write(ironId);
            _packet.Write(health);

            Match match = Server.clients[id].match;

            SendTCPDataToAllMatchInGame(match, _packet);
        }
    }


    public static void SpawnResourcesReceived(int _fromclient, int resourcesType,int amount,Vector3 spawnPos)
    {
        using (Packet _packet = new Packet((int)ServerPackets.SpawnResourcesReceived))
        {
            _packet.Write(resourcesType);
            _packet.Write(amount);
            _packet.Write(spawnPos);

            Match match = Server.clients[_fromclient].match;

            SendTCPDataToAllMatchInGame(match, _packet);
        }
    }

    public static void SpawnResource(int matchId, int resourcesType, int amount, Vector3 spawnPos)
    {
        using (Packet _packet = new Packet((int)ServerPackets.SpawnResourcesReceived))
        {
            _packet.Write(resourcesType);
            _packet.Write(amount);
            _packet.Write(spawnPos);

            Match match = MatchMaker.instance.matches[matchId];

            SendTCPDataToAllMatchInGame(match, _packet);
        }
    }



    public static void PickupResourceReceived(int _fromClient, string resourceId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.PickupResourceReceived))
        {
            _packet.Write(resourceId);

            Match match = Server.clients[_fromClient].match;

            SendTCPDataToAllMatchInGame(match, _packet);
        }
    }
    public static void SpawnSheep(int matchid,string sheepId, Vector3 position, Quaternion rotation)
    {
        using (Packet _packet = new Packet((int)ServerPackets.SpawnSheep))
        {
            _packet.Write(sheepId);
            _packet.Write(position);
            _packet.Write(rotation);

            Match match = MatchMaker.instance.matches[matchid];

            SendTCPDataToAllMatchInGame(match, _packet);
        }
    }

    public static void SheepPosition(int matchId,string sheepId, Vector3 position, Quaternion rotation)
    {
        using (Packet _packet = new Packet((int)ServerPackets.SheepPosition))
        {
            _packet.Write(sheepId);
            _packet.Write(position);
            _packet.Write(rotation);

            Match match = MatchMaker.instance.matches[matchId];

            SendTCPDataToAllMatchInGame(match, _packet);
        }
    }

    public static void SheepAnimation(int matchId, string sheepId,bool isTrigger, string animName, bool animBool = false)
    {
        using (Packet _packet = new Packet((int)ServerPackets.SheepAnimation))
        {
            _packet.Write(sheepId);
            _packet.Write(isTrigger);
            _packet.Write(animName);
            _packet.Write(animBool);

            Match match = MatchMaker.instance.matches[matchId];

            SendTCPDataToAllMatchInGame(match, _packet);
        }
    }

    public static void SpawnStone(int matchId, int randomStone, Vector3 position, Quaternion rotation)
    {
        using (Packet _packet = new Packet((int)ServerPackets.SpawnStone))
        {
            _packet.Write(randomStone);
            _packet.Write(position);
            _packet.Write(rotation);

            Match match = MatchMaker.instance.matches[matchId];

            SendTCPDataToAllMatchInGame(match, _packet);
        }
    }
    public static void SpawnIron(int matchId, Vector3 position, Quaternion rotation)
    {
        using (Packet _packet = new Packet((int)ServerPackets.SpawnIron))
        {
            _packet.Write(position);
            _packet.Write(rotation);

            Match match = MatchMaker.instance.matches[matchId];

            SendTCPDataToAllMatchInGame(match, _packet);
        }
    }

}
public enum ItemType
{
    Sword,
    Axe,
    Bow,
    Pickaxe,
    Shield,
    Arrow,
    Wood,
    Stone,
    Fabric,
    Iron,
    Helmet,
    Armor,
    Boots
}