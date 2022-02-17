using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingBox : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float speed;
    [SerializeField] private bool isDebug;
    [SerializeField] private Camera mainCamera;
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
    private Vector3 DeltaPosition => GetDirection() * Time.deltaTime * speed;

    private Vector3 GetDirection()
    {
        // 원리: https://homo-robotics.tistory.com/16
        
        var mainCameraTransform = mainCamera.transform;
        var cameraAxisY = mainCameraTransform.forward.ConvertXZ();
        var cameraAxisX = mainCameraTransform.right.ConvertXZ();

        var result = cameraAxisX * _input.x + cameraAxisY * _input.y;
        
        return result.ConvertXZ();
    }
}
