using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<string, Health> trees = new Dictionary<string, Health>();
    public static Dictionary<string, Health> stones = new Dictionary<string, Health>();
    public static Dictionary<string, Health> irons = new Dictionary<string, Health>();
    public static Dictionary<string, Resource> resources = new Dictionary<string, Resource>();

    [Header("Players")]
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private GameObject[] localPlayerPrefabs;

    [Header("Camera's")]
    [SerializeField] private CinemachineVirtualCamera followCamera;
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private PotraitCamera potraitCamera;

    [Header("Resources")]
    [SerializeField] private GameObject wood;
    [SerializeField] private GameObject iron;
    [SerializeField] private GameObject fabric;
    [SerializeField] private GameObject stone;

    [Header("Sheep")]
    [SerializeField] private GameObject sheepPrefab;


    [Header("Stone")]
    [SerializeField] private GameObject[] stonePrefabs;

    [Header("Iron")]
    [SerializeField] private GameObject ironPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    void Start()
    {
        ClientSend.JoinedGame();
    }
    public static int spawnCount = 0;
    public void SpawnUser(int id, Character character, Vector3 startPos, Quaternion startRot, int matchId)
    {
        if (players.ContainsKey(id)) return;

        GameObject player;
        if (Client.instance.myId == id)
        {
            player = Instantiate(localPlayerPrefabs[(int)character], startPos, startRot);

            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.InitializeCamera(followCamera,aimCamera);

            CameraControl cameraControl = player.GetComponentInChildren<CameraControl>();
            cameraControl.followCamera = followCamera;
            cameraControl.aimCamera = aimCamera;
            cameraControl.degisimX = followCamera.transform.eulerAngles.x;//kamera dönmesi için
            cameraControl.degisimY = followCamera.transform.eulerAngles.y;

            //potraitCamera.player = player;
            //potraitCamera.gameObject.SetActive(true);
        }
        else
        {
            player = Instantiate(playerPrefabs[(int)character], startPos, startRot);
        }
        player.GetComponent<PlayerManager>().Initialize(id, matchId);
        players.Add(id, player.GetComponent<PlayerManager>());
    }
    public void SpawnResource(ItemType itemType, int amount, Vector3 spawnPos)
    {
        Resource resource = null;
        switch (itemType)
        {

            case ItemType.Wood:
                resource = Instantiate(wood, spawnPos, Quaternion.identity).GetComponent<Resource>();
                break;
            case ItemType.Stone:
                resource = Instantiate(stone, spawnPos, Quaternion.identity).GetComponent<Resource>();
                break;
            case ItemType.Fabric:
                resource = Instantiate(fabric, spawnPos, Quaternion.identity).GetComponent<Resource>();
                break;
            case ItemType.Iron:
                resource = Instantiate(iron, spawnPos, Quaternion.identity).GetComponent<Resource>();
                break;
            default:
                break;
        }

        if (resource != null)
        {
            resource.InitializedValues(itemType, amount);
            resources.Add(resource.resourceId, resource);
        }
    }

    public void DestroyResource(string resourceId)
    {
        Destroy(resources[resourceId].gameObject);
    }
    private Dictionary<string, Sheep> sheeps = new Dictionary<string, Sheep>();
    private Dictionary<string, Animator> sheepAnimators = new Dictionary<string, Animator>();
    public void SpawnSheep(string _sheepId, Vector3 position, Quaternion rotation)
    {
        Sheep sheep = Instantiate(sheepPrefab, position, rotation).GetComponent<Sheep>();
        sheep.sheepId = _sheepId;
        sheeps.Add(_sheepId, sheep);
        sheepAnimators.Add(_sheepId, sheep.GetComponent<Animator>());
    }

    public void SheepPosition(string sheepId, Vector3 position, Quaternion rotation)
    {
        sheeps[sheepId].transform.position = position;
        sheeps[sheepId].transform.rotation = rotation;
    }

    public void SheepAnimation(string sheepId, bool isTrigger, string animName, bool animState)
    {
        if (isTrigger)
        {
            sheepAnimators[sheepId].SetTrigger(animName);
            Destroy(sheeps[sheepId].gameObject, 3);
        }
        else
        {
            sheepAnimators[sheepId].SetBool(animName, animState);
        }
    }

    public void SpawnStone(int stoneTurn, Vector3 position, Quaternion rotation)
    {
        Instantiate(stonePrefabs[stoneTurn], position, rotation);
    }

    public void SpawnIron(Vector3 position, Quaternion rotation)
    {
        Instantiate(ironPrefab, position, rotation);
    }
}
