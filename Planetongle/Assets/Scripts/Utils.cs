using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Utils
{
    public static Vector2 Get2DDirection(Vector3 from, Vector3 to)
    {
        from.z = 0f;
        to.z = 0f;

        return (to - from).normalized;
    }

    public static float GetZAngleDeltaFromDirection(Vector3 dir)
    {
        return (Mathf.Atan2(dir.y, dir.x) * (180f / Mathf.PI)) - 90f;
    }

    public static Vector3 LerpEulerAngles(Vector3 a, Vector3 b, float t)
    {
        float x = Mathf.LerpAngle(a.x, b.x, t);
        float y = Mathf.LerpAngle(a.y, b.y, t);
        float z = Mathf.LerpAngle(a.z, b.z, t);

        return new Vector3(x, y, z);
    }

    public static Vector2 Total(this IEnumerable<ForceData> collection)
    {
        Vector2 total = Vector2.zero;

        foreach (ForceData forceData in collection)
        {
            total += forceData.Force;
        }

        return total;
    }
}
