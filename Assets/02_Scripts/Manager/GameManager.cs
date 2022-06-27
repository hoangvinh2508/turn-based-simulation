using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Game.Objects.Board;
using Game.Objects.Character;

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
        private float _gameSpeed = 1;

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

            var rect = _board.GetBounding();
            var cam = FindObjectOfType<CameraController.CameraController>();
            if (cam != null) {
                cam.SetCameraBounds(rect);
            }
        }

        private void FillDefenseTeam() {
            for (var i = 0; i < _board.BoardRadius / 2; i++) {
                _allies.AddRange(FillCharactersWithRadius(i, _allyPrefab, ref _allyDict));
            }
        }

        private void FillAttackTeam() {
            var size = (_board.BoardRadius - 1) / 2;
            for (var i = 0; i < size; i++) {
                _enemies.AddRange(FillCharactersWithRadius(_board.BoardRadius - 1 - i, _enemyPrefab, ref _enemyDict));
            }
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
            // check defense
            foreach (var ally in _allies) {
                var enemies = GetAllNearByEnemies(ally);
                if (enemies.Length == 0) continue;

                var index = UnityEngine.Random.Range(0, enemies.Length);
                ally.Attack(enemies[index]);
            }
            
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
                    enemy.Move(pos);

                    _enemyDict.Remove(HexCell.CoordinateToIndex(enemy.Coordinate));
                    var index = HexCell.CoordinateToIndex(minCoord);
                    enemy.Coordinate = minCoord;
                    _enemyDict.Add(index, enemy);
                }
            }
            
            // Clean dead character
            var deadAlly = _allies.Where(ch => ch.CurrentHp <= 0).ToArray();
            foreach (var ally in deadAlly) {
                RemoveAlly(ally);
            }

            var deadEnemy = _enemies.Where(ch => ch.CurrentHp <= 0).ToArray();
            foreach (var e in deadEnemy) {
                RemoveEnemy(e);
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
                if (dict.ContainsKey(index) && dict[index].CurrentHp > 0) {
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
            GM.Ui.ShowGameEnd();
        }

        private void UpdatePriority() {
            _allies.Sort(new AllySort());
            _enemies.Sort(new EnemySort());
        }

        // public void RemoveCharacter(Character character, Character.Team team) {
        //     if (team == Character.Team.Attack) {
        //         RemoveEnemy(character);
        //     } else {
        //         RemoveAlly(character);
        //     }
        // }

        private void RemoveAlly(Character character) {
            _allies.Remove(character);
            _allyDict.Remove(HexCell.CoordinateToIndex(character.Coordinate));
        }

        private void RemoveEnemy(Character character) {
            _enemies.Remove(character);
            _enemyDict.Remove(HexCell.CoordinateToIndex(character.Coordinate));
        }

        public void PauseGame() {
            Time.timeScale = 0;
        }

        public void UnPauseGame() {
            Time.timeScale = _gameSpeed;
        }

        public void SetGameSpeed(float value) {
            _gameSpeed = value;
            UnPauseGame();
        }
    }
}