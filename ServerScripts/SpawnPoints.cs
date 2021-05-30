
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 25);
    }
}
