using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {

        string msg = _packet.ReadString();
        int id = _packet.ReadInt();

        Client.instance.myId = id;

        Debug.Log($"Server msg {msg} ");

        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }
    public static void RegisterUserInfoReceived(Packet _packet)
    {
        string msg = _packet.ReadString();
        if (msg == "Success")
        {
            LoginManager.instance.GoLobby();
            return;
        }
        if (msg == "Already")
        {
            LoginManager.instance.ShowMessage("Kullanıcı adı kullanımda");
        }
    }

    public static void LoginUserInfoReceived(Packet _packet)
    {
        string msg = _packet.ReadString();
        if (msg == "Success")
        {
            LoginManager.instance.GoLobby();
            return;
        }
        if (msg == "Dontexist")
        {
            LoginManager.instance.ShowMessage("Kullanıcı adı bulunamadı .");
        }
        else if (msg == "Password")
        {
            LoginManager.instance.ShowMessage("Şifre Yanlış ! ");
        }
    }

    public static void StartGame(Packet packet)
    {
        LobbyUIManager.instance.StartGame();
    }
    //----------------------------------GAME PACKETS
    public static void SpawnUser(Packet packet)
    {
        int userId = packet.ReadInt();
        int character = packet.ReadInt();//farklı karakterler olması ihtimaline karşı
        Vector3 spawnPos = packet.ReadVector3();
        Quaternion spawnRot = packet.ReadQuaternion();
        int matchId = packet.ReadInt();

        GameManager.instance.SpawnUser(userId, (Character)character, spawnPos, spawnRot, matchId);
    }

    public static void PlayerPosition(Packet packet)
    {
        int userId = packet.ReadInt();
        Vector3 currentPos = packet.ReadVector3();

        GameManager.players[userId].transform.position = currentPos;
    }
    public static void PlayerRotation(Packet packet)
    {
        int userId = packet.ReadInt();
        Quaternion currentRot = packet.ReadQuaternion();

        GameManager.players[userId].transform.rotation = currentRot;
        GameManager.players[userId].transform.localEulerAngles = Util.VectorUtil(Vector3.zero, GameManager.players[userId].transform.localEulerAngles.y);
    }

    public static void PlayerAnimationReceived(Packet packet)
    {

        int id = packet.ReadInt();
        string animName = packet.ReadString();
        AnimType animType = (AnimType)packet.ReadInt();
        bool isPlay = packet.ReadBool();
        float value = packet.ReadFloat();

        switch (animType)
        {
            case AnimType.floatAnim:
                GameManager.players[id].animator.SetFloat(animName, value);
                break;
            case AnimType.boolAnim:
                GameManager.players[id].animator.SetBool(animName, isPlay);
                break;
            case AnimType.triggerAnim:
                GameManager.players[id].animator.SetTrigger(animName);
                break;
            default:
                break;
        }

    }

    public static void TreeHealth(Packet _packet)
    {
        string treeId = _packet.ReadString();
        int health = _packet.ReadInt();

        
        GameManager.trees[treeId].SetHealth(health);

    }

    public static void StoneHealth(Packet _packet)
    {
        string stoneId = _packet.ReadString();
        int health = _packet.ReadInt();

        GameManager.stones[stoneId].SetHealth(health);
    }
    public static void IronHealth(Packet _packet)
    {
        string ironId = _packet.ReadString();
        int health = _packet.ReadInt();

        GameManager.irons[ironId].SetHealth(health);
    }
    public static void SpawnResourcesReceived(Packet _packet)
    {
        ItemType itemType = (ItemType)_packet.ReadInt();
        int amount = _packet.ReadInt();
        Vector3 spawnPos = _packet.ReadVector3();

        GameManager.instance.SpawnResource(itemType, amount, spawnPos);
    }

    public static void PickUpResourceReceived(Packet _packet)
    {
        string resourceId = _packet.ReadString();

        GameManager.instance.DestroyResource(resourceId);
    }
    public static void SpawnSheep(Packet _packet)
    {
        string sheepId = _packet.ReadString();
        Vector3 position = _packet.ReadVector3();
        Quaternion rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnSheep(sheepId, position, rotation);
    }
    public static void SheepPosition(Packet _packet)
    {
        string sheepId = _packet.ReadString();
        Vector3 position = _packet.ReadVector3();
        Quaternion rotation = _packet.ReadQuaternion();

        GameManager.instance.SheepPosition(sheepId, position, rotation);
    }

    public static void SheepAnimation(Packet _packet)
    {
        string sheepId = _packet.ReadString();
        bool isTrigger = _packet.ReadBool();
        string animName = _packet.ReadString();
        bool animState = _packet.ReadBool();

        GameManager.instance.SheepAnimation(sheepId, isTrigger, animName, animState);
    }
    public static void SpawnStone(Packet _packet)
    {
        int stoneTurn = _packet.ReadInt();
        Vector3 position = _packet.ReadVector3();
        Quaternion rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnStone(stoneTurn, position, rotation);

    }

    public static void SpawnIron(Packet _packet)
    {
        Vector3 position = _packet.ReadVector3();
        Quaternion rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnIron(position, rotation);
    }

   
}

public enum Character
{
    Farmer = 0,
}

