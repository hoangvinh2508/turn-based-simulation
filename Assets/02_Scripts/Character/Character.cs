using System.Collections;
using System.Collections.Generic;

using Game.Objects.Character;

using UnityEngine;

public class Character : MonoBehaviour {
    [SerializeField]
    private CharacterAnimation _characterAnim = null;

    [SerializeField]
    private int _maxHp = 10;

    private int _hp = 0;
    private int CurrentHp {
        get => _hp;
        set {
            _hp = value;
            // TODO: Update UI
        }
    }

    private void Start() {
        _characterAnim.PlayAnimation(AnimationType.Idle);
        CurrentHp = _maxHp;
    }
}
