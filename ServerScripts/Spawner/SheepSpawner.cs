using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepSpawner : MonoBehaviour
{
    [SerializeField] private GameObject sheepPrefab;
    public float controlTime;
    public int controlOffset;//0 ile 4 arası 0, 1, 2, 3

    private float radius = 600;
    private void Start()
    {
        StartCoroutine(SpawnSheep());
    }
    private IEnumerator SpawnSheep()
    {
        yield return new WaitForSeconds(15);
        while (true)
        {
            yield return new WaitForSeconds(controlTime);
            for (int i = 0; i < MatchMaker.instance.matchCount; i++)
            {
                if (!MatchMaker.instance.matches[i].isStart) continue;
                if (NetworkManager.sheeps.Count <=0) continue;
                if (NetworkManager.sheeps[(MatchMaker.instance. matches[i].id, controlOffset)].Count < NetworkManager.instance.SHEEP_COUNT)
                {
                    for (int s = 0; s < NetworkManager.instance.SHEEP_COUNT; s++)
                    {
                        Vector3 randomDirection = Random.insideUnitSphere * radius;
                        randomDirection += transform.position;
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
                        {
                            Sheep sheep = Instantiate(sheepPrefab, hit.position, Quaternion.identity).GetComponent<Sheep>();
                            sheep.InitializeValues(MatchMaker.instance.matches[i].id, 50, controlOffset);


                            NetworkManager.sheeps[(MatchMaker.instance.matches[i].id, controlOffset)].Add(sheep);
                            NetworkManager.instance.allSheeps.Add(sheep.sheepId, sheep);

                            ServerSend.SpawnSheep(MatchMaker.instance.matches[i].id, sheep.sheepId, hit.position, sheep.transform.rotation);

                            if (NetworkManager.sheeps[(MatchMaker.instance.matches[i].id, controlOffset)].Count >= NetworkManager.instance.SHEEP_COUNT)
                                break;
                        }

                    }
                }
            }
        }
    }
}
