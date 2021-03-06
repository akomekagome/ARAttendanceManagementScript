﻿using UnityEngine;

namespace ARAM.Utils
{    
    public static class Vector3Extensions{
        
        public static Vector3 SetX(this Vector3 origin, float X)
        {
            return new Vector3(X, origin.y, origin.z);
        }

        public static Vector3 SetY(this Vector3 origin, float Y)
        {
            return new Vector3(origin.x, Y, origin.z);
        }

        public static Vector3 SetZ(this Vector3 origin, float Z)
        {
            return new Vector3(origin.x, origin.y, Z);
        }

        public static Vector3 AddX(this Vector3 origin, float X)
        {
            return new Vector3(origin.x + X, origin.y, origin.z);
        }

        public static Vector3 AddY(this Vector3 origin, float Y)
        {
            return new Vector3(origin.x, origin.y + Y, origin.z);
        }

        public static Vector3 AddZ(this Vector3 origin, float Z)
        {
            return new Vector3(origin.x, origin.y, origin.z + Z);
        }

        public static Vector3Int ConvertSquare3D(this Vector3 origin)
        {
            return new Vector3Int(Mathf.FloorToInt(origin.x), Mathf.FloorToInt(origin.y), Mathf.FloorToInt(origin.z));
        }

        public static Vector3 MakeSizeOne(this Vector3 origin)
        {
            if (!Mathf.Approximately(origin.x, 0f)) origin.x = origin.x / Mathf.Abs(origin.x);
            if (!Mathf.Approximately(origin.y, 0f)) origin.y = origin.y / Mathf.Abs(origin.y);
            if (!Mathf.Approximately(origin.z, 0f)) origin.z = origin.z / Mathf.Abs(origin.z);
            return origin;
        }

        public static Vector2 ToXZVector2(this Vector3 origin) => new Vector2(origin.x, origin.z);

        public static Vector3 MeakeOnlyOneAxis(this Vector3 origin)
        {
            if (!Mathf.Approximately(origin.x, 0f)) origin.Set(origin.x, 0f, 0f);
            else if (!Mathf.Approximately(origin.y, 0f)) origin.Set(0f, origin.y, 0f);
            else if (!Mathf.Approximately(origin.z, 0f)) origin.Set(0f, 0f, origin.z);
            return origin;
        }
    }
}
