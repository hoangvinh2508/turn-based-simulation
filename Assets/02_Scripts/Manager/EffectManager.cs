using System.Collections;
using System.Collections.Generic;

using Game.Effect;
using Game.Manager.Base;

using UnityEngine;

namespace Game.Manager {
    public class EffectManager : MonoBehaviour {
        [SerializeField]
        private ObjectPool _hpEffectPool;

        public void ShowHpNumber(int hp, Vector3 pos) {
            var effect = _hpEffectPool.Get<EffectNumber>();
            effect.transform.position = pos;
            effect.Show($"{hp}", () => {
                _hpEffectPool.Put(effect.gameObject);
            });
        }
    }
}