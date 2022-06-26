using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Game.Objects.Board;

using UnityEngine;

namespace Game.Manager {
    class AllySort : IComparer<Character> {
        public int Compare(Character x, Character y) {
            var d1 = x.Coordinate.sqrMagnitude;
            var d2 = y.Coordinate.sqrMagnitude;
            return d2 - d1;
        }
    }

    class EnemySort : IComparer<Character> {
        public int Compare(Character x, Character y) {
            return x.Coordinate.sqrMagnitude - y.Coordinate.sqrMagnitude;
        }
    }
    
    public class GameManager : MonoBehaviour {
        [SerializeField]
        private Board _board;

        [SerializeField]
        private GameObject _allyPrefab = null;

        [SerializeField]
        private GameObject _enemyPrefab = null;

        [SerializeField]
        private Transform _characterContainer = null;
        
        private Dictionary<int, Character> _enemyDict = new ();
        private Dictionary<int, Character> _allyDict = new();

        private List<Character> _enemies = new();
        private List<Character> _allies = new();

        private static Vector2Int[] Directions = {
            new(1, -1),
            new(-1, -1),
            new(-2, 0),
            new(-1, 1),
            new(1, 1),
            new(2, 0)
        };
        
        private void Start() {
            _board.CreateBoard();
            FillDefenseTeam();
            FillAttackTeam();
            UpdatePriority();
        }

        private void FillDefenseTeam() {
            _allies.AddRange(FillCharactersWithRadius(0, _allyPrefab, ref _allyDict));
            _allies.AddRange(FillCharactersWithRadius(1, _allyPrefab, ref _allyDict));
            _allies.AddRange(FillCharactersWithRadius(2, _allyPrefab, ref _allyDict));
        }

        private void FillAttackTeam() {
            _enemies.AddRange(FillCharactersWithRadius(4, _enemyPrefab, ref _enemyDict));
            _enemies.AddRange(FillCharactersWithRadius(5, _enemyPrefab, ref _enemyDict));
        }

        private Character[] FillCharactersWithRadius(int radius, GameObject prefab, ref Dictionary<int, Character> dict) {
            var cells = _board.GetCells(radius);
            var list = new List<Character>();
            foreach (var c in cells) {
                var ch = CreateCharacter(prefab, c.Coordinate);
                ch.Coordinate = c.Coordinate;
                dict.Add(c.Index, ch);
                list.Add(ch);
            }
            return list.ToArray();
        }

        private Character CreateCharacter(GameObject prefab, Vector2Int coordinate) {
            var cell = _board.GetCell(coordinate);
            if (cell == null) {
                return null;
            }
            var obj = Instantiate(prefab, _characterContainer);
            obj.transform.position = cell.transform.position;
            return obj.GetComponent<Character>();
        }

        public void OnGameLoopTrigger() {
            // Check attack
            foreach (var enemy in _enemies) {
                var ally = GetAllyNearby(enemy);
                if (ally != null) {
                    enemy.Attack(ally);
                } else {
                    ally = FindClosestAlly(enemy);
                    if (ally == null) continue;

                    var minCoord = GetClosestCoordinate(enemy, ally);
                    var cell = _board.GetCell(minCoord);
                    var pos = cell.transform.position;
                    enemy.CharacterMove.Move(pos);

                    _enemyDict.Remove(HexCell.CoordinateToIndex(enemy.Coordinate));
                    var index = HexCell.CoordinateToIndex(minCoord);
                    enemy.Coordinate = minCoord;
                    _enemyDict.Add(index, enemy);
                }
            }

            // check defense
            foreach (var ally in _allies) {
                var enemies = GetAllNearByEnemies(ally);
                if (enemies.Length == 0) continue;

                var index = UnityEngine.Random.Range(0, enemies.Length);
                ally.Attack(enemies[index]);
            }

            if (IsGameEnded()) {
                ShowGameEnd();
                return;
            }
            
            UpdatePriority();
        }

        private Character FindClosestAlly(Character target) {
            if (_allies.Count == 0) {
                return null;
            }
            var minDist = float.MaxValue;
            var minIndex = int.MaxValue;
            for (var i = 0; i < _allies.Count; i++) {
                var ally = _allies[i];
                var dist = (ally.Coordinate - target.Coordinate).sqrMagnitude;
                if (dist < minDist) {
                    minDist = dist;
                    minIndex = i;
                }
            }
            return _allies[minIndex];
        }

        private Vector2Int GetClosestCoordinate(Character fromChar, Character toChar) {
            var coord = fromChar.Coordinate;
            var targetCoord = toChar.Coordinate;
            var minCoord = coord;
            var minDist = float.MaxValue;
            foreach (var dir in Directions) {
                var newCoord = coord + dir;
                if (_enemyDict.ContainsKey(HexCell.CoordinateToIndex(newCoord)) || _board.GetCell(newCoord) == null) {
                    continue;
                }
                var dist = (newCoord - targetCoord).sqrMagnitude;
                if (dist < minDist) {
                    minDist = dist;
                    minCoord = newCoord;
                }
            }
            return minCoord;
        }

        private Character GetAllyNearby(Character enemy) {
            return GetCharacterNearby(enemy, Character.Team.Defense);
        }

        private Character GetCharacterNearby(Character target, Character.Team team) {
            var coord = target.Coordinate;
            var dict = team == Character.Team.Attack ? _enemyDict : _allyDict;
            foreach (var dir in Directions) {
                var newCoord = coord + dir;
                var index = HexCell.CoordinateToIndex(newCoord);
                if (dict.ContainsKey(index)) {
                    return dict[index];
                }
            }
            return null;
        }

        private Character[] GetAllNearByEnemies(Character target) {
            var coord = target.Coordinate;
            var list = new List<Character>();
            foreach (var dir in Directions) {
                var newCoord = coord + dir;
                var index = HexCell.CoordinateToIndex(newCoord);
                if (_enemyDict.ContainsKey(index)) {
                    list.Add(_enemyDict[index]);
                }
            }
            return list.ToArray();
        }

        private bool IsGameEnded() {
            return _enemyDict.Count == 0 || _allyDict.Count == 0;
        }

        public void ShowGameEnd() {
            
        }

        private void UpdatePriority() {
            _allies.Sort(new AllySort());
            _enemies.Sort(new EnemySort());
        }

        private void RemoveCharacter(Character character, Character.Team team) {
            if (team == Character.Team.Attack) {
                _enemies.Remove(character);
                _enemyDict.Remove(HexCell.CoordinateToIndex(character.Coordinate));
            } else {
                _allies.Remove(character);
                _allyDict.Remove(HexCell.CoordinateToIndex(character.Coordinate));
            }

            Destroy(character.gameObject);
        }
    }
}