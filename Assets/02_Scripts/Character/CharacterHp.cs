using System;

using UnityEngine;

namespace Game.Objects.Character {
    public class CharacterHp : MonoBehaviour {
        [SerializeField]
        private SpriteRenderer _hpRenderer;

        [SerializeField]
        private float _maxSize = 4;

        [SerializeField]
        private float _animSpeed = 5;

        private int _maxHp = 0;

        public int MaxHp {
            get => _maxHp;
            set {
                _maxHp = value;
                _currentHp = _maxHp;
                _displayHp = _maxHp;
            }
        }

        private int _currentHp = 0;
        private float _displayHp = 0;
        private bool _isHpChanging = false;

        public int CurrentHp {
            get => _currentHp;
            set {
                _currentHp = value;
                _isHpChanging = true;
            }
        }

        public void Update() {
            if (!_isHpChanging) return;

            _displayHp = Mathf.Lerp(_displayHp, _currentHp, Time.deltaTime * _animSpeed);
            if (Mathf.Abs(_currentHp - _displayHp) <= Mathf.Epsilon) {
                _displayHp = _currentHp;
                _isHpChanging = false;
            }
            SetHpProgress(_displayHp / MaxHp * 1.0f);
        }

        private void SetHpProgress(float progress) {
            var size = _hpRenderer.size;
            size.x = progress * _maxSize;
            _hpRenderer.size = size;
        }
    }
}