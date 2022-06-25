using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Objects.Board {
    public class HexCell : MonoBehaviour {
        [SerializeField]
        public Vector2Int _coordinate = Vector2Int.zero;
    }
}