using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingBox : MonoBehaviour
{
    [SerializeField, Range(0, 100f)] private float speed;
    [SerializeField] private bool isDebug;
    private Vector2 _input;

    [UsedImplicitly]
    private void OnMove(InputValue value)
    {
        _input = value.Get<Vector2>();
        ShowLog();
    }

    private void ShowLog()
    {
        if (isDebug) Debug.Log($"X: {_input.x}, Y: {_input.y}");
    }
    
    private void Update() => MoveBox();
    private void MoveBox() => transform.position += DeltaPosition;
    private Vector3 DeltaPosition => _input.ConvertXZ() * Time.deltaTime * speed;
}
