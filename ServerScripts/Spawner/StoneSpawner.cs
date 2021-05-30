using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] stonePrefab;
    public float stoneCount;

    private Dictionary<int, List<GameObject>> stones;
    private void Awake()
    {
        stones = new Dictionary<int, List<GameObject>>();
    }
    private float radius = 600;
    public void SpawnStone(int matchId)
    {
        List<GameObject> stoneClones = new List<GameObject>();
        for (int i = 0; i < stoneCount; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                int randomStone = Random.Range(0, stonePrefab.Length);
                GameObject stone = Instantiate(stonePrefab[randomStone], hit.position, Quaternion.identity);

                stoneClones.Add(stone);
                ServerSend.SpawnStone(matchId, randomStone, hit.position, stone.transform.rotation);
            }
        }
        stones.Add(matchId, stoneClones);
    }
    public void DeleteMatchStones(int matchId)
    {
        for (int i = 0; i < stones[matchId].Count; i++)
        {
            Destroy(stones[matchId][i]);
        }
        stones.Remove(matchId);
    }
}
