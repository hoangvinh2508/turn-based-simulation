using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace Game.Objects.Board {
    public class Board : MonoBehaviour {
        [SerializeField]
        private int _boardRadius = 10;

        [SerializeField]
        private float _cellRadius = 5;

        [SerializeField]
        private GameObject _cellPrefab = null;

        private Dictionary<int, HexCell> _matrix = new Dictionary<int, HexCell>();

        public int BoardRadius => _boardRadius;

        public void CreateBoard() {
            var height = (_boardRadius - 1) * 2 + 1;
            var width = height * 2 - 1;
            var midX = width / 2;
            for (var y = -_boardRadius + 1; y < _boardRadius; y++) {
                var h = Mathf.Abs(y);
                for (var x = h; x <= width - h; x += 2) {
                    CreateCell(x - midX, y);
                }
            }
        }

        private void CreateCell(int x, int y) {
            var obj = Instantiate(_cellPrefab, transform);
            var cell = obj.GetComponent<HexCell>();
            cell.Coordinate = new Vector2Int(x, y);

            var pos = Vector3.zero;
            pos.x = x * _cellRadius;
            pos.y = y * _cellRadius * 1.732f;
            cell.transform.position = pos;

            _matrix.Add(HexCell.CoordinateToIndex(cell.Coordinate), cell);
        }

        public HexCell GetCell(Vector2Int coordinate) {
            var key = HexCell.CoordinateToIndex(coordinate);
            return GetCell(key);
        }

        public HexCell GetCell(int index) {
            return _matrix.ContainsKey(index) ? _matrix[index] : null;
        }

        public HexCell[] GetCells(int radius) {
            if (radius == 0) {
                return new[] { GetCell(0) };
            }
            var pivots = new Vector2Int[] {
                new(radius, radius),
                new(radius * 2, 0),
                new(radius, -radius),
                new(-radius, -radius),
                new(-radius * 2, 0),
                new(-radius, radius)
            };

            var dirs = new Vector2Int[] {
                new(1, -1),
                new(-1, -1),
                new(-2, 0),
                new(-1, 1),
                new(1, 1),
                new(2, 0)
            };

            var pivotCount = pivots.Length;
            var index = 0;
            var coord = pivots[index];
            var cells = new List<HexCell>();
            
            while (true) {
                var nextIndex = (index + 1) % pivotCount;
                var target = pivots[nextIndex];
                cells.Add(GetCell(coord));

                coord += dirs[index];
                if (coord == target) {
                    if (nextIndex == 0) {
                        break;
                    }
                    index++;
                }
            }

            return cells.ToArray();
        }
    }
}