using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotraitCamera : MonoBehaviour
{
    public GameObject player;
    private Vector3 diff = new Vector3(.2f, .9f, 3f);
    private GameObject environment;
    private void Start()
    {
        environment = GameObject.Find("ENVIRONMENT");
        environment.SetActive(true);
        transform.SetParent(player.transform);
    }
    private void LateUpdate()
    {
        transform.position = player.transform.position + diff;
    }
}
