using System.Collections;
using System.Collections.Generic;

using Game.Manager.Base;

using UnityEngine;

namespace Game.Manager {
    public class GM : Singleton<GM> {
        [SerializeField]
        private GameManager _gameManager = null;

        [SerializeField]
        private InputManager _inputManager = null;

        [SerializeField]
        private EffectManager _effectManager = null;

        [SerializeField]
        private UIManager _uiManager = null;

        public static GameManager Game {
            get
            {
                Initialize();
                return Instance._gameManager;
            }
        }

        public static InputManager Input {
            get {
                Initialize();
                return Instance._inputManager;
            }
        }

        public static EffectManager Effect {
            get {
                Initialize();
                return Instance._effectManager;
            }
        }
        
        public static UIManager Ui {
            get {
                Initialize();
                return Instance._uiManager;
            }
        }

        private static bool _initialized = false;

        private static void Initialize() {
            if (_initialized) {
                return;
            }

            _initialized = true;
        }
    }   
}