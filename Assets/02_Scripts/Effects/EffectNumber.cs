using System;

using TMPro;

using UnityEngine;

namespace Game.Effect {
    public class EffectNumber : MonoBehaviour {
        [SerializeField]
        private TextMeshPro _text = null;

        [SerializeField]
        private Animator _animator = null;

        private Action _callback = null;

        public void Show(string text, Action callback = null) {
            _text.text = text;
            _animator.Play("Text_Fly");
            _callback = callback;
        }

        public void OnAnimationPlayDone() {
            _callback?.Invoke();
        }
    }
}