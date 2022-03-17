using UnityEngine;
using UnityEngine.InputSystem;

namespace Sana
{
    // TODO (숙제): OnCameraRotate, OnCameraTilt 함수 완성하기
    class CameraController : MonoBehaviour
    {
        [SerializeField]                 Transform mainCamera;
        [SerializeField, Range(0f, 10f)] float     zoomSpeed         = 10f;
        [SerializeField, Range(0f, 10f)] float     zoomStep          = 5f;
        [SerializeField, Range(0f, 50f)] float     defaultCameraZoom = 20f;
        [SerializeField, Range(0f, 50f)] float     minCameraZoom     = 3f;
        [SerializeField, Range(0f, 50f)] float     maxCameraZoom     = 50f;

        float _currentZoomLevel;
        float _targetZoomLevel;

        [SerializeField] Transform pivot;

        [SerializeField, Range(0f, 10f)] float tiltSpeed        = 5f;
        [SerializeField, Range(0f, 10f)] float tiltStep         = 1f;
        [SerializeField, Range(0f, 90f)] float defaultTiltAngle = 45f;
        [SerializeField, Range(0f, 90f)] float minTiltAngle     = 0f;
        [SerializeField, Range(0f, 90f)] float maxTiltAngle     = 85f;

        float _currentTiltLevel;
        float _targetTiltLevel;

        [SerializeField, Range(0f,   10f)] float rotationSpeed        = 5f;
        [SerializeField, Range(0f,   10f)] float rotationStep         = 0.25f;
        [SerializeField, Range(-90f, 90f)] float defaultRotationAngle = 0f;
        [SerializeField, Range(-90f, 0f)]  float minRotationAngle     = -90f;
        [SerializeField, Range(0f,   90f)] float maxRotationAngle     = 90f;

        float _currentRotationLevel;
        float _targetRotationLevel;

        void OnValidate()
        {
            float newMin = ClampDistance(minCameraZoom);
            float newMax = ClampDistance(maxCameraZoom);

            minCameraZoom = newMin;
            maxCameraZoom = newMax;

            defaultCameraZoom = ClampDistance(defaultCameraZoom);

            _currentZoomLevel        = defaultCameraZoom;
            _targetZoomLevel         = _currentZoomLevel;
            
            mainCamera.localPosition = new Vector3(0f, 0f, -_currentZoomLevel);
        }

        float ClampDistance(float value)
        {
            return Mathf.Clamp(value, minCameraZoom, maxCameraZoom);
        }

        float ClampAngle(float value)
        {
            return Mathf.Clamp(value, minTiltAngle, maxTiltAngle);
        }

        float ClampRotation(float value)
        {
            return Mathf.Clamp(value, minRotationAngle, maxRotationAngle);
        }

        void OnCameraRotate(InputValue input)
        {
            // OnCameraRotate(InputValue input) 에 따라 CameraPivot 의 local y축을 회전
            var value = input.Get<float>();

            // 입력 값을 디버그 콘솔에 출력
            Debug.Log($"Rotate: {value}");
            
            if (value == 0) return;

            _targetRotationLevel += -value * rotationStep;
            _targetRotationLevel =  ClampRotation(_targetRotationLevel);
        }

        void OnCameraTilt(InputValue input)
        {
            // OnCameraTilt(InputValue input) 에 따라 CameraPivot 의 local x축을 회전
            var value = input.Get<float>();

            // 입력 값을 디버그 콘솔에 출력
            Debug.Log($"Tilt: {value}");

            if (value == 0) return;

            _targetTiltLevel += -value * tiltStep;
            _targetTiltLevel =  ClampAngle(_targetTiltLevel);
        }

        void OnCameraZoom(InputValue input)
        {
            // OnCameraZoom(InputValue input) 에 따라 MainCamera 의 local z축을 이동
            var value = input.Get<float>();

            // 입력 값을 디버그 콘솔에 출력
            Debug.Log($"Zoom: {value}");

            // 마우스 스크롤이 멈춘 경우 return
            if (value == 0) return;

            // 줌 레벨 (=카메라 로컬 z위치)를 설정
            _targetZoomLevel += +-value * zoomStep;

            // 카메라 거리를 일정 범위 내에 있게 만들기
            _targetZoomLevel = ClampDistance(_targetZoomLevel);
        }

        void Update()
        {
            // Lerp를 이용해 현제 줌 레벨을 목표 줌 레벨로 천천히 변환
            _currentZoomLevel = Mathf.Lerp(
                _currentZoomLevel,
                _targetZoomLevel,
                zoomSpeed * Time.deltaTime);

            // 줌 레벨을 설정
            mainCamera.localPosition = new Vector3(0f, 0f, -_currentZoomLevel);

            // Lerp를 이용해 현제 줌 레벨을 목표 줌 레벨로 천천히 변환
            _currentTiltLevel = Mathf.Lerp(
                _currentTiltLevel,
                _targetTiltLevel,
                tiltSpeed * Time.deltaTime);
            _currentRotationLevel = Mathf.Lerp(
                _currentRotationLevel,
                _targetRotationLevel,
                rotationSpeed * Time.deltaTime);

            // 줌 레벨을 설정
            pivot.localEulerAngles = new Vector3(_currentTiltLevel, -_currentRotationLevel, 0f);
        }
    }
}