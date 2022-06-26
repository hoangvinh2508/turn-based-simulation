using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Game.Objects.Board;

using UnityEngine;

namespace Game.Manager {
    public class GameManager : MonoBehaviour {
        [SerializeField]
        private Board _board;

        [SerializeField]
        private GameObject _allyPrefab = null;

        [SerializeField]
        private GameObject _enemyPrefab = null;

        [SerializeField]
        private Transform _characterContainer = null;

        private List<Character> _enemies = new List<Character>();
        private List<Character> _allies = new List<Character>();
        
        private void Start() {
            _board.CreateBoard();
            FillDefenseTeam();
            FillAttackTeam();
        }

        private void FillDefenseTeam() {
            _allies.AddRange(FillCharactersWithRadius(0, _allyPrefab));
            _allies.AddRange(FillCharactersWithRadius(1, _allyPrefab));
            _allies.AddRange(FillCharactersWithRadius(2, _allyPrefab));
        }

        private void FillAttackTeam() {
            _enemies.AddRange(FillCharactersWithRadius(4, _enemyPrefab));
            _enemies.AddRange(FillCharactersWithRadius(5, _enemyPrefab));
        }

        private IEnumerable<Character> FillCharactersWithRadius(int radius, GameObject prefab) {
            var cells = _board.GetCells(radius);
            var list = cells.Select(c => CreateCharacter(prefab, c.Coordinate)).ToList();
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

        private void Update() {
            // TODO:
        }

        public void OnGameLoopTrigger() {
            // TODO: Attack or defense
        }
    }
}