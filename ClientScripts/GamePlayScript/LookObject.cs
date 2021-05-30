using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookObject : MonoBehaviour
{
    [SerializeField] private new GameObject camera;
    private void Update()
    {
        transform.position = camera.transform.forward * 50+camera.transform.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 20);
    }
}
