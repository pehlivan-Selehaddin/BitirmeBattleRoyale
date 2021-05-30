using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Match
{
    public readonly int id;
    public List<Client> clients;
    public List<bool> isSpawn;
    public List<int> inGamePlayers;
    public bool isStart = false;

    public Match(int _id)
    {
        id = _id;
        clients = new List<Client>();
        isSpawn = new List<bool>();
        inGamePlayers = new List<int>();

        for (int i = 0; i < NetworkManager.instance.spawnPoints.Length; i++)
        {
            isSpawn.Add(false);
        }
    }
}

