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

        private Camera _camera = null;
        private float _currentZoomValue = 0;
        private Transform _transform = null;
        private Vector3 _startPosition = Vector3.zero;
        private Vector3 _velocity = Vector3.zero;

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
                Mathf.Lerp(_camera.orthographicSize, _currentZoomValue, Time.deltaTime * _zoomSpeed);
            
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
            _transform.position = pos;
            // TODO: check limit camera
        }
    }
}