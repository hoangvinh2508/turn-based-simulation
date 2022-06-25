using UnityEngine;

namespace Game.Manager.Base {
    public class Singleton <T>: MonoBehaviour where T : MonoBehaviour {
        private static T _instance = null;

        protected virtual void Awake() {
            if (_instance == null) {
                _instance = this as T;
            } else if (_instance != this) {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        public static T Instance => _instance;
    }
}