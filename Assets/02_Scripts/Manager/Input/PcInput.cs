using UnityEngine;

using UInput = UnityEngine.Input;

namespace Game.Manager.Input {
    public class PcInput : IInput {
        public void Update() {
            
        }

        public float GetZoomValue() {
            return UInput.GetAxis("Mouse ScrollWheel");
        }

        public bool IsPressStart() {
            return UInput.GetMouseButtonDown(0);
        }

        public bool IsPressing() {
            return UInput.GetMouseButton(0);
        }

        public bool IsPressEnd() {
            return UInput.GetMouseButtonUp(0); 
        }

        public Vector3 GetPointerPosition(Camera camera) {
            return camera.ScreenToWorldPoint(UInput.mousePosition);
        }
    }
}