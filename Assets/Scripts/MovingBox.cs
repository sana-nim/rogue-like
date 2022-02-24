using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: 카메라 각도 바꾸는 조작 추가
// TODO: 건설 크래프팅 생존 게임

[RequireComponent(typeof(CharacterController))]
public class MovingBox : MonoBehaviour
{
    #region SerializedFields
    
    [SerializeField] private bool isDebug;
    [SerializeField] private Camera mainCamera;
    
    [Header("Values")]
    [SerializeField, Range(0f, 50f)] private float speed = 10f;
    [SerializeField, Range(0f, 50f)] private float maxYVelocity = 5f;
    [SerializeField, Range(-50f, -5f)] private float gravity = -10f;
    [SerializeField, Range(1, 4)] private int maxJumpTimes = 2;
    
    #endregion
    
    private CharacterController _cc;
    private Vector2 _input;
    private float _currentYVelocity;
    private int _jumpCount;
    

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

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
        if (isDebug) Debug.Log($"Joystick X: {_input.x}, Y: {_input.y}");
    }
    
    [UsedImplicitly]
    private void OnJump(InputValue value)
    {
        if (_cc.isGrounded) _jumpCount = 0;
        if (_jumpCount == maxJumpTimes) return;
        
        _currentYVelocity = maxYVelocity;
        _jumpCount++;

        if (isDebug) Debug.Log($"Jump {_jumpCount} times!");
    }

    #endregion

    #region Movement

    private void MoveBox()
    {
        Vector3 deltaPosition = GetDirection() * Time.fixedDeltaTime * speed;
        _cc.Move(deltaPosition);
    }

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
        var deltaSpeed = gravity * Time.fixedDeltaTime;
        _currentYVelocity += deltaSpeed;

        var yDeltaPosition = _currentYVelocity * Time.fixedDeltaTime;

        var deltaPosition = new Vector3(0f, yDeltaPosition, 0f);
        _cc.Move(deltaPosition);

        LimitPlayerVelocity();
    }
    
    private void LimitPlayerVelocity()
    {
        if (_cc.isGrounded) _currentYVelocity = 0f;
    }
    
    #endregion
}
