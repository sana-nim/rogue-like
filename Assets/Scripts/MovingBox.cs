using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingBox : MonoBehaviour
{
    [SerializeField] private bool isDebug;
    [SerializeField] private Camera mainCamera;
    
    [Header("Values")]
    [SerializeField, Range(0f, 50f)] private float speed;
    [SerializeField, Range(0f, 3f)] private float maxYVelocity;
    [SerializeField, Range(-50f, -5f)] private float gravity;
    
    private Vector2 _input;
    private float _currentYVelocity;

    private void FixedUpdate()
    {
        MoveBox();
        ProcessJump();
    }
    
    #region Input
    
    [UsedImplicitly]
    private void OnMove(InputValue value)
    {
        _input = value.Get<Vector2>();
        if (isDebug) Debug.Log($"X: {_input.x}, Y: {_input.y}");
    }
    
    [UsedImplicitly]
    private void OnJump(InputValue value)
    {
        if (value.isPressed) _currentYVelocity = maxYVelocity;
        if (isDebug) Debug.Log("Pressed Jump");
    }

    #endregion

    #region Movement

    private void MoveBox() => transform.position += DeltaPosition;
    
    private Vector3 DeltaPosition => GetDirection() * Time.fixedDeltaTime * speed;

    private Vector3 GetDirection()
    {
        // 원리: https://homo-robotics.tistory.com/16
        
        Transform mainCameraTransform = mainCamera.transform;
        Vector2 cameraAxisY = mainCameraTransform.forward.ConvertXZ();
        Vector2 cameraAxisX = mainCameraTransform.right.ConvertXZ();

        Vector2 result = cameraAxisX * _input.x + cameraAxisY * _input.y;
        
        return result.ConvertXZ();
    }

    #endregion

    #region Jump
    
    private void ProcessJump()
    {
        _currentYVelocity += gravity * Time.fixedDeltaTime;
        
        if (_currentYVelocity < 0f) _currentYVelocity = 0f;

        Vector3 position = transform.position;
        position.y += _currentYVelocity + gravity * Time.fixedDeltaTime;
        transform.position = position;
        
        LimitPlayerHeight();
    }
    
    private void LimitPlayerHeight()
    {
        if (transform.localPosition.y > 0f) return;
        
        Vector3 position = transform.localPosition;
        position.y = 0f;
        transform.localPosition = position;
    }
    
    #endregion
}
