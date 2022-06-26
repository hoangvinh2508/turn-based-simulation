using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Manager.Base {
    public class ObjectPool : MonoBehaviour {
        [SerializeField]
        private GameObject _pooledObject = null;

        [SerializeField]
        private int _numberInPool = 5;

        [SerializeField]
        private Transform _targetTransform = null;

        private List<GameObject> _actives = new List<GameObject>();
        private List<GameObject> _inactives = new List<GameObject>();

        public List<GameObject> ActiveObjects => _actives;

        private void Start() {
            for (var i = 0; i < _numberInPool; i++) {
                Create();
            }
        }

        private GameObject Create() {
            var obj = Instantiate(_pooledObject, _targetTransform);
            obj.SetActive(false);
            _inactives.Add(obj);
            return obj;
        }

        public T Get<T>() where T : MonoBehaviour {
            if (_inactives.Count == 0) {
                Create();
            }
            var obj = _inactives[^1];
            _inactives.RemoveAt(_inactives.Count - 1);
            obj.SetActive(true);
            _actives.Add(obj);
            return obj.GetComponent<T>();
        }

        public void Put(GameObject obj) {
            obj.SetActive(false);
            _inactives.Add(obj);
            _actives.Remove(obj);
        }
    }

}