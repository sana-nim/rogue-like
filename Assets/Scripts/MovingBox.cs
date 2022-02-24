using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingBox : MonoBehaviour
{
    [SerializeField] private bool isDebug;
    [SerializeField] private Camera mainCamera;
    
    [Header("Values")]
    [SerializeField, Range(0f, 50f)] private float speed;
    [SerializeField, Range(0f, 50f)] private float maxYVelocity;
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
        // TODO: 이단 점프 막기
        // TODO: 중력이 월드 방향에 영향받기 만들기
        // TODO: 월드 경계랑 충돌하게 하기
        // TODO: 환경오브젝트랑 충돌하게 하기
        // TODO: 월드 회전 기믹 구현
        // TODO: 카메라 각도 바꾸는 조작 추가
        
        // TODO: 건설 크래프팅 생존 게임

        var deltaSpeed = gravity * Time.fixedDeltaTime;
        _currentYVelocity += deltaSpeed;

        Vector3 position = transform.position;
        var deltaPosition = _currentYVelocity * Time.fixedDeltaTime;
        position.y += deltaPosition;
        transform.position = position;
        
        LimitPlayerHeight();
    }
    
    private void LimitPlayerHeight()
    {
        if (transform.localPosition.y > 0f) return;
        
        Vector3 position = transform.localPosition;
        position.y = 0f;
        _currentYVelocity = 0f;
        transform.localPosition = position;
    }
    
    #endregion
}
