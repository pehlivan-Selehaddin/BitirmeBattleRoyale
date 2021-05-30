using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public int SHEEP_COUNT = 10;

    public static Dictionary<(int, int), List<Sheep>> sheeps = new Dictionary<(int, int), List<Sheep>>();

    [Header("STONES")]
    public StoneSpawner[] stoneSpawners;

    [Header("IRONS")]
    public IronSpawner[] ironSpawners;



    [Header("PLAYER"),Space(30)]
    public GameObject playerPrefab;
    [Space(30)]
    public Transform[] spawnPoints;

    [Header("SHEEP"), Space(30)]
    public GameObject sheepPrefab;
    public GameObject[] SheepSpawnOffsets;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists , destroying objects .. ");

            Destroy(this);
        }
    }
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(50, 6060);
    }


    private void OnApplicationQuit()
    {
        Server.Stop();
    }
    public void CallRegister(int clientId, string username, string password)
    {
        StartCoroutine(Register(clientId, username, password));
    }
    IEnumerator Register(int clientId, string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("password", password);
        UnityWebRequest request = UnityWebRequest.Post("http://localhost/BitirmeSql/Register.php", form);
        yield return request.SendWebRequest();

        string requestText = request.downloadHandler.text;
        if (string.IsNullOrEmpty(requestText))
        {
            Debug.Log("User created Successfully");
            ServerSend.RegisterUserInfoReceived(clientId, "Success");
        }
        else
        {
            Debug.Log("User creation  Failed Error # " + request.downloadHandler.text);
            if (requestText.Contains("Already"))
            {
                ServerSend.RegisterUserInfoReceived(clientId, "Already");
            }
        }
    }



    public void CallLogin(int clientId, string username, string password)
    {
        StartCoroutine(Login(clientId, username, password));
    }

    public Player IntantiatePlayer(int id)
    {
        Debug.Log("Instantiate player");
        Match userMatch = Server.clients[id].match;

        Vector3 position = GetRandomSpawnPoint(userMatch).position;
        Quaternion rotation = playerPrefab.transform.rotation;

        Player player = Instantiate(playerPrefab, position, Quaternion.identity).GetComponent<Player>();

        player.startPos = position;
        player.startRot = rotation;

        return player;
    }

    private Transform GetRandomSpawnPoint(Match match)
    {
        int random;
        while (true)
        {
            random = UnityEngine.Random.Range(0, spawnPoints.Length);

            if (match.isSpawn[random]) continue;

            match.isSpawn[random] = true;

            return spawnPoints[random];
        }
    }
    IEnumerator Login(int clientId, string username, string password)//bakılacak
    {
        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("password", password);
        UnityWebRequest request = UnityWebRequest.Post("http://localhost/BitirmeSql/Login.php", form);
        yield return request.SendWebRequest();

        string requestText = request.downloadHandler.text;
        if (requestText.Contains("id"))
        {
            Debug.Log("User Login Successfully");

            Server.clients[clientId].DBid = Convert.ToInt32(requestText.Substring(3));//database id sinin eşleşmesi

            ServerSend.LoginUserInfoReceived(clientId, "Success");
        }
        else
        {
            Debug.Log("User Login  Failed Error # " + requestText);

            if (requestText.Contains("3"))
            {
                ServerSend.LoginUserInfoReceived(clientId, "Dontexist");//kullanıcı adı bulunamadı
            }
            else if (requestText.Contains("4"))
            {
                ServerSend.LoginUserInfoReceived(clientId, "Password");//şifre yanlış
            }
        }
    }

    public void HitIron(int fromClient, string ironId, int damage)
    {
        Server.clients[fromClient].player.HitIron(ironId, damage);
    }

    public void HitStone(int fromClient, string stoneId, int damage)
    {
        Server.clients[fromClient].player.HitStone(stoneId, damage);
    }

    public void HitTree(int fromClient, string treeId, int damage)
    {
        Server.clients[fromClient].player.HitTree(treeId, damage);
    }

    public void DestroyPlayer(int id)
    {
        Destroy(Server.clients[id].player.gameObject);
    }
    private float radius = 600;
    public void SpawnSheeps(int matchid)
    {
        List<Sheep> sheepList;
        for (int i = 0; i < 4; i++)
        {
            sheepList = new List<Sheep>();
            for (int j = 0; j < SHEEP_COUNT; j++)
            {
                Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
                randomDirection += SheepSpawnOffsets[i].transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
                {
                    Sheep sheep = Instantiate(sheepPrefab, hit.position, Quaternion.identity).GetComponent<Sheep>();
                    sheep.InitializeValues(matchid, 50, i);
                    sheepList.Add(sheep);

                    allSheeps.Add(sheep.sheepId, sheep);

                    ServerSend.SpawnSheep(matchid, sheep.sheepId, hit.position, sheep.transform.rotation);
                }
            }
            sheeps.Add((matchid, i), sheepList);
        }
    }
    public Dictionary<string, Sheep> allSheeps = new Dictionary<string, Sheep>();
    public void HitSheep(string sheepId, int damage)
    {
        if (allSheeps.ContainsKey(sheepId))
        {
            allSheeps[sheepId].Damage(damage);
        }
    }

    public void ClearSheepData(int matchId)
    {
        if (sheeps.Count > 0)
            for (int i = 0; i < 4; i++)
            {
                foreach (var item in sheeps[(matchId, i)])
                {
                    Destroy(item.gameObject);
                    allSheeps.Remove(item.sheepId);
                }
                sheeps.Remove((matchId, i));
            }
    }


    public void SpawnStones(int matchId)
    {
        for (int i = 0; i < stoneSpawners.Length; i++)
        {
            stoneSpawners[i].SpawnStone(matchId);
        }
    }

    public void ClearStoneData(int matchId)
    {
        for (int i = 0; i < stoneSpawners.Length; i++)
        {
            stoneSpawners[i].DeleteMatchStones(matchId);
        }
    }
    public void SpawnIrons(int matchId)
    {
        for (int i = 0; i < ironSpawners.Length; i++)
        {
            ironSpawners[i].SpawnIron(matchId);
        }
    }

    public void ClearIronData(int matchId)
    {
        for (int i = 0; i < ironSpawners.Length; i++)
        {
            ironSpawners[i].DeleteMatchIrons(matchId);
        }
    }
}
