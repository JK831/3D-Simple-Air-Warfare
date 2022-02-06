using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculate
{
    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public static Vector3 rotateVectorByAngle(Vector3 vector1, Vector3 vector2, float angle)
    {
        return Vector3.Normalize(vector1 * Mathf.Cos(angle) + vector2 * Mathf.Sin(angle));
    }

  
}
