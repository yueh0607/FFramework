using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityEx
{

    /// <summary>
    /// 设置并返回
    /// </summary>
    /// <param name="v"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Vector3 SetX(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    /// <summary>
    /// 设置并返回
    /// </summary>
    /// <param name="v"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector3 SetY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }
    /// <summary>
    /// 设置并返回
    /// </summary>
    /// <param name="v"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static Vector3 SetZ(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }

    //Vector2
    public static Vector2 SetX(this Vector2 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector2 SetY(this Vector2 v, float y)
    {
        v.y = y;
        return v;
    }


}
