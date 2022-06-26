using UnityEngine;

namespace Game.Objects.Character {
    public class Skill {
        public void ProccessSkill(Character target) {
            var attackNumber = Random.Range(0, 3);
            var defenseNumber = Random.Range(0, 3);
            var value = 3 + attackNumber - defenseNumber;
            if (value % 3 == 0) {
                DealHp(target, 4);
            } else if (value % 3 == 1) {
                DealHp(target, 5);
            } else if (value % 3 == 2) {
                DealHp(target, 3);
            }
        }

        public void DealHp(Character target, int hp) {
            target.OnHit(hp);
        }
    }
}