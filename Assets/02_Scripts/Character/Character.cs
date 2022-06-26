using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using Game.Manager;
using Game.Objects.Character;

using UnityEngine;

namespace Game.Objects.Character {
    public class Character : MonoBehaviour {
        public enum Team {
            Defense,
            Attack
        }

        [SerializeField]
        private CharacterAnimation _characterAnim = null;

        [SerializeField]
        private int _maxHp = 10;

        [SerializeField]
        private CharacterMove _move = null;

        [SerializeField]
        private CharacterHp _hpBar = null;

        [SerializeField]
        private Team _team = Team.Attack;
        
        private Skill _skill = new();
        public Vector2Int Coordinate { get; set; } = Vector2Int.zero;

        private int _hp = 0;

        public int CurrentHp {
            get => _hp;
            set {
                _hp = Mathf.Max(value, 0);
                _hpBar.CurrentHp = _hp;
                if (_hp <= 0) {
                    GM.Game.RemoveCharacter(this, _team);
                    PlayDieAndFadeOut();
                }
            }
        }

        private void Start() {
            _characterAnim.PlayAnimation(AnimationType.Idle);
            _hpBar.MaxHp = _maxHp;
            CurrentHp = _maxHp;
        }

        public void Move(Vector3 position) {
            _characterAnim.PlayAnimation(AnimationType.Move);
            _move.Move(position, () => {
                _characterAnim.PlayAnimation(AnimationType.Idle);
            });
        }

        public void OnHit(int hp) {
            GM.Effect.ShowHpNumber(hp, transform.position + new Vector3(Random.Range(0, 2), Random.Range(0, 2) + 3, 0));
            CurrentHp -= hp;
            _characterAnim.PlayAnimation(AnimationType.Hit, () => {
                _characterAnim.PlayAnimation(AnimationType.Idle);
            });
        }

        public void Attack(Character target) {
            _skill.ProccessSkill(target);
        }

        public void PlayDieAndFadeOut() {
            _characterAnim.PlayAnimation(AnimationType.Die);
            _characterAnim.FadeOut(() => {
                gameObject.SetActive(false);
            });
        }
    }
}