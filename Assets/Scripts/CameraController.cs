using UnityEngine;
using UnityEngine.InputSystem;

namespace Sana
{
    // TODO (숙제): 레콘이 적은 수상한 코드 보고 이해하려고 노력하기
    // TODO (숙제): OnCameraRotate, OnCameraTilt 함수 완성하기
    class CameraController : MonoBehaviour
    {
        [SerializeField] Transform pivot;
        [SerializeField] Transform mainCamera;
        [SerializeField, Range(0f,10f)] float zoomSpeed = 5f;
        [SerializeField, Range(0f,10f)] float zoomStep = 1f;

        float _currentZoomLevel;
        float _targetZoomLevel;

        // OnCameraRotate(InputValue input) 에 따라 CameraPivot 의 local y축을 회전
        void OnCameraRotate(InputValue input)
        {
            var value = input.Get<float>();
            
            // 입력 값을 디버그 콘솔에 출력
            Debug.Log($"Rotate: {value}");
        }

        // OnCameraTilt(InputValue input) 에 따라 CameraPivot 의 local x축을 회전
        void OnCameraTilt(InputValue input)
        {
            var value = input.Get<float>();
            
            // 입력 값을 디버그 콘솔에 출력
            Debug.Log($"Tilt: {value}");
        }

        // OnCameraZoom(InputValue input) 에 따라 MainCamera 의 local z축을 이동
        void OnCameraZoom(InputValue input)
        {
            var value = input.Get<float>();
            
            // 입력 값을 디버그 콘솔에 출력
            Debug.Log($"Zoom: {value}");

            // 마우스 스크롤이 멈춘 경우 return
            if (value == 0) return;

            // 줌 레벨 (=카메라 로컬 z위치)를 설정
            _targetZoomLevel = _currentZoomLevel + value * zoomStep;

            // 아래와 같은 기능을 하는 코드
            // if (_targetZoomLevel > 0) _targetZoomLevel = 0;
            // if (_targetZoomLevel < -30) _targetZoomLevel = -30;
            _targetZoomLevel = Mathf.Clamp(_targetZoomLevel, -30f, 0f);
        }

        void Update()
        {
            // Lerp를 이용해 현제 줌 레벨을 목표 줌 레벨로 천천히 변환
            _currentZoomLevel = Mathf.Lerp(
                _currentZoomLevel, 
                _targetZoomLevel, 
                zoomSpeed * Time.deltaTime);
            
            // 줌 레벨을 설정
            mainCamera.localPosition = new Vector3(0f, 0f, _currentZoomLevel);
        }
    }
}