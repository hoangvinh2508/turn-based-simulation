using System;
using System.Collections;
using System.Collections.Generic;

using Game.Manager.Input;

using UnityEngine;

namespace Game.Manager {
    public class InputManager : MonoBehaviour {
        private IInput _input;

        private void Start() {
#if UNITY_ANDROID || UNITY_IOS
            _input = new MobileInput();
#else
            _input = new PcInput();
#endif
        }

        private void Update() {
            _input.Update();
        }

        public float GetZoomValue() => _input.GetZoomValue();
        public Vector3 GetPointerPosition(Camera camera) => _input.GetPointerPosition(camera);
        public bool IsPressStart() => _input.IsPressStart();
        public bool IsPressing() => _input.IsPressing();
        public bool IsPressEnd() => _input.IsPressEnd();
    }
}