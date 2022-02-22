using System;
using UnityEngine;

public class AutoRotator : MonoBehaviour
{
    private enum Axis
    {
        X,
        Y,
        Z,
    }
    
    [SerializeField] private Axis axis;
    [SerializeField, Range(0f, 100f)] private float speed;

    private void Update()
    {
        Vector3 rotationAxis = axis switch
        {
            Axis.X => Vector3.right,
            Axis.Y => Vector3.up,
            Axis.Z => Vector3.forward,
            _ => throw new ArgumentOutOfRangeException(),
        };

        transform.Rotate(rotationAxis, speed * Time.deltaTime);
    }
}
