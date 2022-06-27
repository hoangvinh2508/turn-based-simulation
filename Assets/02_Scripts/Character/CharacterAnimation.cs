using System;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using Game.Config;

using Spine.Unity;

using UnityEngine;

namespace Game.Objects.Character {
    public enum AnimationType {
        Idle,
        Hit,
        Attack,
        Move,
        Die,
    }
    public class CharacterAnimation : MonoBehaviour {
        [SerializeField]
        private SkeletonAnimation _animation;

        [SerializeField]
        private CharacterConfig _config;

        private void Start() {
            _animation.Skeleton.a = 1;
        }

        public void PlayAnimation(AnimationType type, Action callback = null) {
            var state = _animation.AnimationState;
            var animName = type switch {
                AnimationType.Attack => _config.AttackAnimName,
                AnimationType.Die => _config.DieAnimName,
                AnimationType.Hit => _config.HitAnimName,
                AnimationType.Idle => _config.IdleAnimName,
                AnimationType.Move => _config.MoveAnimName,
                _ => ""
            };
            var loop = type == AnimationType.Idle;
            var entry = state.SetAnimation(0, animName, loop);
            if (!loop) {
                entry.Complete += trackEntry => {
                    callback?.Invoke();
                };
            }
        }

        public void FadeOut(TweenCallback callback = null) {
            DOTween.To(() => _animation.Skeleton.a, x => _animation.Skeleton.a = x, 0, 0.5f).OnComplete(callback);
        }
    }
}