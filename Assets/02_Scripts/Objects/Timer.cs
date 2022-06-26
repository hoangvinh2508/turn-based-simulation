using UnityEngine;
using UnityEngine.Events;

namespace Game.Objects {
    public class Timer : MonoBehaviour {
        [SerializeField]
        private float _triggerTime = 1;

        [SerializeField]
        private float _speedMultiplier = 1;

        [SerializeField]
        private UnityEvent _onTrigger = new UnityEvent();

        private float _time = 0;
        private bool _paused = false;

        public float SpeedMultiplier {
            get => _speedMultiplier;
            set => _speedMultiplier = value;
        }

        public float TriggerTime {
            get => _triggerTime;
            set => _triggerTime = value;
        }

        private void Start() {
            _time = _triggerTime;
        }

        private void Update() {
            if (_paused) {
                return;
            }

            _time -= Time.deltaTime * _speedMultiplier;
            if (_time <= 0) {
                _time = _triggerTime;
                _onTrigger?.Invoke();
            }
        }

        public void SetPause(bool pause) {
            _paused = pause;
        }

        public void SetSpeedMultiplier(float value) {
            SpeedMultiplier = value;
        }
    }
}