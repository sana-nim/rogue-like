using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sana
{
    public class AutoRotator : MonoBehaviour
    {
        enum Axis
        {
            X,
            Y,
            Z,
        }
    
        [SerializeField] Axis axis;
        [SerializeField, Range(-100f, 100f)] float speed;
        [SerializeField] bool isShake;

        void Update()
        {
            Vector3 rotationAxis = axis switch
            {
                Axis.X => Vector3.right,
                Axis.Y => Vector3.up,
                Axis.Z => Vector3.forward,
                _ => throw new ArgumentOutOfRangeException(),
            };
        
            if (isShake)
            {
                transform.Rotate(rotationAxis, Random.Range(-speed, speed) * Time.deltaTime);
            }
            else
            {
                transform.Rotate(rotationAxis, speed * Time.deltaTime);
            }
        }
    }
}
