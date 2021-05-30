using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deneme : MonoBehaviour
{
    private void Start()
    {
        Camera camera = GetComponent<Camera>();
        float[] distances = new float[32];
        for (int i = 0; i < 32; i++)
        {
            distances[i] = 500;
        }
        camera.layerCullDistances = distances;
    }
}
