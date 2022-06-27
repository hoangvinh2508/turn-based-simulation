using System;
using System.Collections;
using System.Collections.Generic;

using Game.CameraController;

using UnityEngine;

public class BackgroundSprite : MonoBehaviour {
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private SpriteRenderer _sprite;

    private Transform _transform = null;

    private void Awake() {
        _transform = transform;
    }

    private void LateUpdate() {
        var height = _camera.orthographicSize;
        var width = height * _camera.aspect;

        var size = _sprite.size;
        var sx = width / size.x;
        var sy = height / size.y;
        var s = Mathf.Max(sx, sy, 1);
        _transform.localScale = new Vector3(s, s, s);
    }
}
