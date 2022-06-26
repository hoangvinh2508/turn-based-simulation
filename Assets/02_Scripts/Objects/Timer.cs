using System;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Objects {
    public class Timer : MonoBehaviour {
        [SerializeField]
        private float _triggerTime = 1;

        [SerializeField]
        private UnityEvent _onTrigger = new UnityEvent();

        private float _time = 0;
        private bool _paused = false;

        private void Start() {
            _time = _triggerTime;
        }

        private void Update() {
            if (_paused) {
                return;
            }

            _time -= Time.deltaTime;
            if (_time <= 0) {
                _time = _triggerTime;
                _onTrigger?.Invoke();
            }
        }

        public void SetPause(bool pause) {
            _paused = pause;
        }
    }
}