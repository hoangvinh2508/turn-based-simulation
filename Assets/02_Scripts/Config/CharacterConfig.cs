using UnityEngine;

namespace Game.Config {
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Config/Character Config", order = 0)]
    public class CharacterConfig : ScriptableObject {
        [SerializeField]
        private string _idleAnimName = "action/idle";

        [SerializeField]
        private string _hitAnimName = "";

        [SerializeField]
        private string _attackAnimName = "";

        [SerializeField]
        private string _moveAnimName = "";

        [SerializeField]
        private string _dieAnimName = "";

        [SerializeField]
        private string _hitBoneName = "";

        public string IdleAnimName => _idleAnimName;

        public string HitAnimName => _hitAnimName;

        public string AttackAnimName => _attackAnimName;

        public string MoveAnimName => _moveAnimName;

        public string DieAnimName => _dieAnimName;

        public string HitBoneName => _hitBoneName;
    }
}