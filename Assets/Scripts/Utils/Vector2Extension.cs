using UnityEngine;

public static class Vector2Extension
{
    public static Vector3 ConvertXZ(this Vector2 input)
    {
        Vector3 result = default;
        
        result.x = input.x;
        result.y = 0f;
        result.z = input.y;
        
        return result;
    }
}