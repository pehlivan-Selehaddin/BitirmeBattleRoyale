using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    

    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.WelcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write("Usename asdasd");

            SendTCPData(_packet);
        }
    }

 

    public static void RegisterUserInfo(string username, string password)
    {
        using (Packet _packet = new Packet((int)ClientPackets.RegisterUserInfo))
        {
            _packet.Write(username);
            _packet.Write(password);

            SendTCPData(_packet);
        }

    }

  

    public static void LoginUserInfo(string username, string password)
    {
        using (Packet _packet = new Packet((int)ClientPackets.LoginUserInfo))
        {
            _packet.Write(username);
            _packet.Write(password);

            SendTCPData(_packet);
        }
    }


    public static void FindMatch()
    {
        using (Packet _packet = new Packet((int)ClientPackets.FindMatch))
        {
            SendTCPData(_packet);
        }
    }

    public static void JoinedGame()
    {
        using (Packet _packet = new Packet((int)ClientPackets.JoinedGame))
        {
            SendTCPData(_packet);
        }
    }

    public static void PlayerMoveDirection(Vector3 moveDirection,bool isMoving)
    {
        using (Packet _packet = new Packet((int)ClientPackets.PlayerMoveDirection))
        {
            _packet.Write(moveDirection);
            _packet.Write(isMoving);
            SendTCPData(_packet);
        }
    }
    public static void PlayerAnimation(string animName, AnimType animType,bool isPlay=false,float value=0)
    {
        using (Packet _packet = new Packet((int)ClientPackets.PlayerAnimation))
        {
            _packet.Write(animName);
            _packet.Write((int)animType);
            _packet.Write(isPlay);
            _packet.Write(value);

            SendTCPData(_packet);
        }
    }
    public static void PlayerInAim(Vector3 direction,AimType aimtype,bool isMoving,Vector3 moveDirection)
    {
        using (Packet _packet = new Packet((int)ClientPackets.PlayerInAim))
        {
            _packet.Write(direction);
            _packet.Write((int)aimtype);
            _packet.Write(isMoving);
            _packet.Write(moveDirection);

            SendTCPData(_packet);
        }
    }
    public static void HitTree(int damage,string treeId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.HitTree))
        {
            _packet.Write(damage);
            _packet.Write(treeId);

            SendTCPData(_packet);
        }
    }

    public static void HitSheep(int damage, string sheepId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.HitSheep))
        {
            _packet.Write(damage);
            _packet.Write(sheepId);

            SendTCPData(_packet);
        }
    }

    public static void HitStone(int damage, string stoneId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.HitStone))
        {
            _packet.Write(damage);
            _packet.Write(stoneId);

            SendTCPData(_packet);
        }
    }

    public static void HitIron(int damage, string ironId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.HitIron))
        {
            _packet.Write(damage);
            _packet.Write(ironId);

            SendTCPData(_packet);
        }
    }

    public static void SpawnResources(ItemType itemType,int amount,Vector3 spawnPos)
    {
        using (Packet _packet = new Packet((int)ClientPackets.SpawnResources))
        {
            _packet.Write((int)itemType);
            _packet.Write(amount);
            _packet.Write(spawnPos);

            SendTCPData(_packet);
        }
    }

    public static void PickUpResource(string resourceId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.PickUpResource))
        {
            _packet.Write(resourceId);

            SendTCPData(_packet);
        }
    }
}
public enum AnimType
{
    floatAnim=0,
    boolAnim=1,
    triggerAnim=2,
}
public enum AimType
{
    BowAim=0,
    PistolAim=1,
}
