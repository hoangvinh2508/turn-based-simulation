using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Objects.Board {
    public class Board : MonoBehaviour {
        [SerializeField]
        private int _boardRadius = 10;

        [SerializeField]
        private float _cellRadius = 5;

        // Inner Cell Radius = Radius * Mathf.Sqrt(3) / 2
        public float InnerCellRadius => _cellRadius * 0.866025404f;
    }
}