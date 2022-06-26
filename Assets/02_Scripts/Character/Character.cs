using System.Collections;
using System.Collections.Generic;

using Game.Objects.Character;

using UnityEngine;

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
    private Team _team = Team.Attack;

    public CharacterMove CharacterMove => _move;
    public Team CharacterTeam => _team;

    public Vector2Int Coordinate { get; set; } = Vector2Int.zero;
    
    private int _hp = 0;
    
    private int CurrentHp {
        get => _hp;
        set {
            _hp = value;
        }
    }

    private void Start() {
        _characterAnim.PlayAnimation(AnimationType.Idle);
        CurrentHp = _maxHp;
    }

    public void Attack(Character other) {
        
    }
}
