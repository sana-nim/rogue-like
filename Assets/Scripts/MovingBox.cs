using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingBox : MonoBehaviour
{
    private Vector2 _input;
    
    void Update()
    {
        var pos = transform.position;

        var newPos = GetNewPos(pos);

        transform.position = newPos;
    }

    Vector3 GetNewPos(Vector3 pos)
    {
        Vector3 newPos = pos;
        
        // Write code here
        newPos.x += (_input.x*Time.deltaTime);
        newPos.z += (_input.y*Time.deltaTime);

        return newPos;
    }

    void OnMove(InputValue value)
    {
        _input = value.Get<Vector2>();
        
        // W: (0, 1)
        // A: (-1, 0)
        // S: (0, -1)
        // D: (1, 0)
        // Joystick: (-1 ~ 1, -1 ~ 1)
        
        Debug.Log($"X: {_input.x}, Y: {_input.y}");
    }
}
