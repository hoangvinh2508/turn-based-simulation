using System;

using UnityEngine;

namespace Game.Objects.Character {
    public class CharacterMove : MonoBehaviour {
        [SerializeField]
        private float _moveSpeed = 10;

        private Vector3 TargetPosition { get; set; } = Vector3.zero;

        private Transform _transform;
        private Vector3 _position;
        private bool _paused = false;
        private bool _isMove = false;
        private Action _callback = null;

        private void Awake() {
            _transform = transform;
            _position = _transform.position;
        }

        public void Move(Vector3 pos, Action callback = null) {
            TargetPosition = pos;
            _position = _transform.position;
            _isMove = true;
            _callback = callback;
        }

        private void Update() {
            if (!_isMove) return;
            
            _position = Vector3.Lerp(_position, TargetPosition, Time.deltaTime * _moveSpeed);
            _transform.position = _position;
            if (_position == TargetPosition) {
                _isMove = false;
                _callback?.Invoke();
            }
        }
    }
}