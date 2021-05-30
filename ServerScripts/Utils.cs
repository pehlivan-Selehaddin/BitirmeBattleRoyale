using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Vector3 HeightCorrected(this Vector3 vector, float y)
    {
        vector.y = y;
        return vector;
    }


}
