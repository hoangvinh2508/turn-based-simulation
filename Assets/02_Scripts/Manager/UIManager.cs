using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Manager {
    public class UIManager : MonoBehaviour {
        [SerializeField]
        private GameObject _buttonPause = null;

        [SerializeField]
        private GameObject _pausedGameObject = null;

        [SerializeField]
        private TextMeshProUGUI _speedText = null;

        [SerializeField]
        private GameObject _gameEndGameObject = null;

        [SerializeField]
        private float[] _gameSpeeds;

        private int _speedIndex = 1;

        private void Start() {
            UpdateSpeedText();
            _pausedGameObject.SetActive(false);
            _gameEndGameObject.SetActive(false);
        }

        public void OnButtonPaused() {
            GM.Game.PauseGame();
            _buttonPause.SetActive(false);
            _pausedGameObject.SetActive(true);
        }

        public void OnButtonUnPause() {
            _pausedGameObject.SetActive(false);
            _buttonPause.SetActive(true);
            GM.Game.UnPauseGame();
        }

        public void OnButtonSpeed() {
            _speedIndex = (_speedIndex + 1) % _gameSpeeds.Length;
            var speed = _gameSpeeds[_speedIndex];
            GM.Game.SetGameSpeed(speed);
            UpdateSpeedText();
        }

        public void OnButtonReplay() {
            Destroy(GM.Instance.gameObject);
            SceneManager.LoadScene(0);
        }

        public void ShowGameEnd() {
            if (_gameEndGameObject.activeSelf) {
                return;
            }
            _gameEndGameObject.SetActive(true);
        }

        private void UpdateSpeedText() {
            _speedText.text = $"Speed: x{_gameSpeeds[_speedIndex]}";
        }
    }
}