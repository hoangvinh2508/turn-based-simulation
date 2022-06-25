using UnityEngine;
using UInput = UnityEngine.Input;

namespace Game.Manager.Input {
    public class MobileInput : IInput {
        private float _zoomValue = 0;

        public void Update() {
            var touch0 = UInput.touches[0];
            var touch1 = UInput.touches[1];

            var lastPos0 = touch0.position - touch0.deltaPosition;
            var lastPos1 = touch1.position - touch1.deltaPosition;

            var lastDelta = (lastPos0 - lastPos1).magnitude;
            var nowDelta = (touch0.position - touch1.position).magnitude;
            var value = nowDelta - lastDelta;
            if (Mathf.Abs(value) >= 0.02f) {
                _zoomValue = value;
            } else {
                _zoomValue = 0;
            }
        }

        public float GetZoomValue() {
            return _zoomValue;
        }

        public bool IsPressStart() {
            return UInput.touchCount > 0 && UInput.touches[0].phase == TouchPhase.Began;
        }

        public bool IsPressing() {
            if (UInput.touchCount == 0) {
                return false;
            }
            var phase = UInput.touches[0].phase;
            return phase == TouchPhase.Stationary || phase == TouchPhase.Moved;
        }

        public bool IsPressEnd() {
            return UInput.touchCount > 0 && UInput.touches[0].phase == TouchPhase.Ended;
        }

        
        public Vector3 GetPointerPosition(Camera camera) {
            return UInput.touchCount == 0 ? new Vector3(-1000, -1000, -1000) : camera.ScreenToWorldPoint(UInput.touches[0].position);
        }
    }
}