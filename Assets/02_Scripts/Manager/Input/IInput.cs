using UnityEngine;

namespace Game.Manager.Input {
    public interface IInput {
        void Update();
        float GetZoomValue();
        bool IsPressStart();
        bool IsPressing();
        bool IsPressEnd();
        Vector3 GetPointerPosition(Camera camera);
    }
}