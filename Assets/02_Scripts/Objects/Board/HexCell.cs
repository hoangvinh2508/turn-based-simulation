using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

namespace Game.Objects.Board {
    public class HexCell : MonoBehaviour {
        [SerializeField]
        private Vector2Int _coordinate = Vector2Int.zero;

        [SerializeField]
        private TextMeshPro _debugText = null;
        
        public Vector2Int Coordinate {
            get => _coordinate;
            set {
                _coordinate = value;
                SetDebugText($"{_coordinate.x} {_coordinate.y}");
            }
        }

        public int Index => CoordinateToIndex(Coordinate);

        public void SetDebugText(string text) {
            _debugText.text = text;
        }

        public static int CoordinateToIndex(Vector2Int coord) {
            return coord.x * 100000 + coord.y;
        }

        public static Vector2Int IndexToCoordinate(int index) {
            return new Vector2Int(index / 100000, index % 100000);
        }
    }
}