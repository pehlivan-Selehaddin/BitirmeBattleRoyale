using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
public class MatchMaker : MonoBehaviour
{
    public static MatchMaker instance;
    public int matchCount = 100;
    public int matchClientCount = 2;
    public Match[] matches;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeMatchData();
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void FindMatch(int _id)//
    {
        for (int i = 0; i < matches.Length; i++)
        {
            if (matches[i].clients.Count < matchClientCount)//clienti bu matche ekle
            {
                matches[i].clients.Add(Server.clients[_id]);//disconnect olduğunda sil match den
                Server.clients[_id].match = matches[i];//clientin matchini verdik
                //Debug.Log(matches[i].clients.Count);
                if (matches[i].clients.Count == matchClientCount)
                {
                    matches[i].isStart = true;
                    StartGame(matches[i]);
                }
                return;
            }
        }
    }

    public async void StartGame(Match _match)
    {
        ServerSend.StartGame(_match);

        await WaitForSeconds(7000);

        NetworkManager.instance.SpawnIrons(_match.id);
        NetworkManager.instance.SpawnStones(_match.id);
        NetworkManager.instance.SpawnSheeps(_match.id);

        // _match.clients = new List<Client>();//match sıfırlandı yeniden eklenebilir hale geldi.//match veri gönderiminde lazım
    }
    private async static Task WaitForSeconds(int milliseconds)
    {
        await Task.Delay(milliseconds);
    }
    public void InitializeMatchData()
    {
        matches = new Match[matchCount];
        for (int i = 0; i < matchCount; i++)
        {
            matches[i] = new Match(i);
        }

    }
}
