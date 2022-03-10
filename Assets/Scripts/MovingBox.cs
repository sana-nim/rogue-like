using JetBrains.Annotations;
using Sana.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sana
{
    [RequireComponent(typeof(CharacterController))]
    class MovingBox : MonoBehaviour
    {
        #region SerializedFields

        [SerializeField] bool isDebug;
        [SerializeField] Camera mainCamera;

        [Header("Values")] [SerializeField, Range(0f, 50f)]
        float speed = 10f;

        [SerializeField, Range(0f, 50f)] float maxYVelocity = 5f;
        [SerializeField, Range(-50f, -5f)] float gravity = -10f;
        [SerializeField, Range(1, 4)] int maxJumpTimes = 2;

        #endregion

        CharacterController _cc;
        Vector2 _input;
        float _currentYVelocity;
        int _jumpCount;


        void Awake()
        {
            _cc = GetComponent<CharacterController>();
        }

        void FixedUpdate()
        {
            MoveBox();
            ProcessJump();
        }

        #region Input

        [UsedImplicitly]
        void OnMove(InputValue value)
        {
            _input = value.Get<Vector2>();
            if (isDebug) Debug.Log($"Joystick X: {_input.x}, Y: {_input.y}");
        }

        [UsedImplicitly]
        void OnJump(InputValue value)
        {
            if (_cc.isGrounded) _jumpCount = 0;
            if (_jumpCount == maxJumpTimes) return;

            _currentYVelocity = maxYVelocity;
            _jumpCount++;

            if (isDebug) Debug.Log($"Jump {_jumpCount} times!");
        }

        #endregion

        #region Movement

        void MoveBox()
        {
            Vector3 deltaPosition = GetDirection() * (Time.fixedDeltaTime * speed);
            _cc.Move(deltaPosition);
        }

        Vector3 GetDirection()
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

        void ProcessJump()
        {
            var deltaSpeed = gravity * Time.fixedDeltaTime;
            _currentYVelocity += deltaSpeed;

            var yDeltaPosition = _currentYVelocity * Time.fixedDeltaTime;

            var deltaPosition = new Vector3(0f, yDeltaPosition, 0f);
            _cc.Move(deltaPosition);

            LimitPlayerVelocity();
        }

        void LimitPlayerVelocity()
        {
            if (_cc.isGrounded) _currentYVelocity = 0f;
        }

        #endregion
    }
}