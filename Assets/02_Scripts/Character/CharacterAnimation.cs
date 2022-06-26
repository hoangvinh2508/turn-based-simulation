using System.Collections;
using System.Collections.Generic;

using Game.Config;

using Spine.Unity;

using UnityEngine;

namespace Game.Objects.Character {
    public enum AnimationType {
        Idle,
        Hit,
        Attack,
        Move,
    }
    public class CharacterAnimation : MonoBehaviour {
        [SerializeField]
        private SkeletonAnimation _animation;

        [SerializeField]
        private CharacterConfig _config;

        public void PlayAnimation(AnimationType type) {
            var state = _animation.AnimationState;
            switch (type) {
                case AnimationType.Idle: {
                    state.SetAnimation(0, _config.IdleAnimName, true);
                    break;
                }
                case AnimationType.Attack: {
                    state.SetAnimation(0, _config.AttackAnimName, false);
                    break;
                }
                case AnimationType.Hit: {
                    state.SetAnimation(0, _config.HitAnimName, false);
                    break;
                }
                case AnimationType.Move: {
                    state.SetAnimation(0, _config.MoveAnimName, true);
                    break;
                }
            }
        }
    }
}