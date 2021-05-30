using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IronSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ironPrefab;
    public float ironCount;

    private Dictionary<int, List<GameObject>> irons;
    private void Awake()
    {
        irons = new Dictionary<int, List<GameObject>>();
    }
    private float radius = 600;
    public void SpawnIron(int matchId)
    {
        List<GameObject> ironClones = new List<GameObject>();
        for (int i = 0; i < ironCount; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                GameObject iron = Instantiate(ironPrefab, hit.position, Quaternion.identity);
                ironClones.Add(iron);
                ServerSend.SpawnIron(matchId, hit.position, iron.transform.rotation);
            }
        }
        irons.Add(matchId, ironClones);
    }
    public void DeleteMatchIrons(int matchId)
    {
        for (int i = 0; i < irons[matchId].Count; i++)
        {
            Destroy(irons[matchId][i]);
        }
        irons.Remove(matchId);
    }
}

