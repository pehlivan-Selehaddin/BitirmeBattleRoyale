using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Sheep : MonoBehaviour
{
    public string sheepId;
    public int matchId;
    public int spawnOffset;
    public Health health;

    private NavMeshAgent agent;
    private Animator animator;
    private WaitForSeconds waitGameStart;

    public void InitializeValues(int _matchId, int _health, int _spawnOffset)
    {
        sheepId = Guid.NewGuid().ToString();
        matchId = _matchId;
        spawnOffset = _spawnOffset;
        health = new Health(_health);

        waitGameStart = new WaitForSeconds(15);

       StartCoroutine(SendPosition());
    }
    private WaitForSeconds fixedTime;
    private IEnumerator SendPosition()
    {
        fixedTime = new WaitForSeconds(Time.fixedDeltaTime);
        yield return waitGameStart;
        while (true)
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            ServerSend.SheepPosition(matchId,sheepId, position, rotation);

            if (health.GetHealth()<=0)
            {
                NetworkManager.sheeps[(matchId, spawnOffset)].Remove(this);
                NetworkManager.instance.allSheeps.Remove(sheepId);

                StopCoroutine(SheepControl());
                StopCoroutine(PatrolControl());

                ServerSend.SheepAnimation(matchId, sheepId, true, "die");

                int randomFabricAmount = UnityEngine.Random.Range(1, 5);
                ServerSend.SpawnResource(matchId, (int)ItemType.Fabric, randomFabricAmount, transform.position.RandomPosition(3,transform.position.y+1));

                Destroy(gameObject);
            }
            yield return fixedTime;
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        StartCoroutine(SheepControl());
        StartCoroutine(PatrolControl());
    }
    public void Damage(int _damage)
    {
        health.Damage(_damage);
    }

    private WaitForSeconds delaySheepControl;
    private WaitForSeconds delayPatrolControl;
    private IEnumerator SheepControl()
    {
        delaySheepControl = new WaitForSeconds(.5f);
        yield return waitGameStart;
        while (true)
        {
            yield return delaySheepControl;
            if (agent.remainingDistance < 0.1f)
            {
                animator.SetBool("run", false);
                animator.SetBool("idle", true);
                ServerSend.SheepAnimation(matchId,sheepId,false, "run", false);
                ServerSend.SheepAnimation(matchId, sheepId, false, "idle", true);
            }
            else
            {
                animator.SetBool("idle", false);
                ServerSend.SheepAnimation(matchId, sheepId, false, "idle", false);
            }
            See();
        }
    }
    private IEnumerator PatrolControl()
    {
        delayPatrolControl = new WaitForSeconds(10f);
        yield return  waitGameStart;
        while (true)
        {
            Patrol();
            yield return delayPatrolControl;
        }
    }
    private bool isSee = false;

    private void See()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 30);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                Player player = colliders[i].GetComponent<Player>();
                if (player == null) continue;
                if (Server.clients[player.id].match.id == matchId)
                {
                    if (agent.remainingDistance < 1f)
                    {
                        agent.destination = transform.position + transform.forward * 10;
                    }
                    agent.speed = 4;
                    ServerSend.SheepAnimation(matchId, sheepId, false, "run", true);
                    isSee = true;
                    return;
                }
            }
        }
        isSee = false;
    }
    private float walkRadius = 20;
    private void Patrol()
    {
        if (isSee) return;

        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;

        agent.SetDestination(finalPosition);

        agent.speed = 2;
        animator.SetBool("idle", false);
    }
}
public static class VectorExtension
{
    public static Vector3 RandomPosition(this Vector3 offset,float size,float y)
    {
        float valueX = UnityEngine.Random.Range(-size, size);
        float valueZ = UnityEngine.Random.Range(-size, size);

        offset.x += valueX;
        offset.z += valueZ;
        offset.y = y;

        return offset;
    }
}