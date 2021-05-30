using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static Vector3 VectorUtil(Vector3 point, float y)
    {
        point.y = y;
        return point;
    }
    public static Vector3 GetRandomPosition(Vector3 offset, float size)
    {
        float valueX = Random.Range(-size, size);
        float valueZ = Random.Range(-size, size);

        offset.x += valueX;
        offset.z += valueZ;

        return offset;
    }
}
