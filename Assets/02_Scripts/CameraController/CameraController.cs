using System;

using Game.Manager;

using UnityEngine;

namespace Game.CameraController {
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour {
        [SerializeField]
        private float _zoomFactor = 3;

        [SerializeField]
        private float _zoomSpeed = 10;

        [SerializeField]
        private Vector2 _zoomLimit = new Vector2(2, 30);

        [SerializeField]
        private Rect _cameraBounds;

        private Camera _camera = null;
        private float _currentZoomValue = 0;
        private Transform _transform = null;
        private Vector3 _startPosition = Vector3.zero;
        private Vector3 _velocity = Vector3.zero;

        private Rect _cameraLimit = new();

        private void Awake() {
            _camera = GetComponent<Camera>();
            _transform = transform;
            _currentZoomValue = _camera.orthographicSize;
        }

        private void LateUpdate() {
            var zoomValue = GM.Input.GetZoomValue();
            _currentZoomValue -= zoomValue * _zoomFactor;
            _currentZoomValue = Mathf.Clamp(_currentZoomValue, _zoomLimit.x, _zoomLimit.y);
            _camera.orthographicSize =
                Mathf.Lerp(_camera.orthographicSize, _currentZoomValue, Time.unscaledTime * _zoomSpeed);
            UpdateCameraLimit();
            
            if (zoomValue <= 0.01f) {
                if (GM.Input.IsPressStart()) {
                    // check drag camera
                    _startPosition = GM.Input.GetPointerPosition(_camera);
                    _velocity = Vector3.zero;
                }
            }

            if (_velocity.magnitude >= 0.01f) {
                MoveCameraWithDelta(_velocity);
                _velocity *= 0.95f;
            } else {
                _velocity = Vector3.zero;
            }

            if (GM.Input.IsPressEnd()) {
                _velocity = _startPosition - GM.Input.GetPointerPosition(_camera);
            }

            if (!GM.Input.IsPressing()) {
                return;
            }

            var delta = _startPosition - GM.Input.GetPointerPosition(_camera);
            MoveCameraWithDelta(delta);
        }
        
        private void MoveCameraWithDelta(Vector3 delta) {
            var pos = _transform.position;
            delta.z = 0;
            pos += delta;

            pos.x = Mathf.Clamp(pos.x, _cameraLimit.min.x, _cameraLimit.max.x);
            pos.y = Mathf.Clamp(pos.y, _cameraLimit.min.y, _cameraLimit.max.y);
            
            _transform.position = pos;
            // TODO: check limit camera
        }

        public void SetCameraBounds(Rect bound) {
            _cameraBounds = bound;
        }

        private void UpdateCameraLimit() {
            var height = _camera.orthographicSize;
            var width = height * _camera.aspect;
            _cameraLimit.x = _cameraBounds.x + width / 2;
            _cameraLimit.width = Mathf.Max(_cameraBounds.width - width, 0);
            _cameraLimit.y = _cameraBounds.y + height / 2;
            _cameraLimit.height = Mathf.Max(_cameraBounds.height - height, 0);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            var a1 = _cameraBounds.min;
            var a2 = new Vector3(_cameraBounds.min.x, _cameraBounds.max.y, 0);
            var a3 = _cameraBounds.max;
            var a4 = new Vector3(_cameraBounds.max.x, _cameraBounds.min.y, 0);
            Gizmos.DrawLine(a1, a2);
            Gizmos.DrawLine(a2, a3);
            Gizmos.DrawLine(a3, a4);
            Gizmos.DrawLine(a4, a1);
        }
#endif
    }
}