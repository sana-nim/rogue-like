using UnityEngine;

namespace Sana.Utils
{
    public static class Vector3Extension
    {
        public static Vector2 ConvertXZ(this Vector3 input)
        {
            Vector2 result = default;
        
            result.x = input.x;
            result.y = input.z;

            return result;
        }
    }
}