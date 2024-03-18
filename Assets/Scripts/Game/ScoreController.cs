using System;
using UnityEngine;

namespace Game
{
    public class ScoreController : MonoBehaviour
    {
        public event Action<int> ScoreChanged;

        [SerializeField] private AudioSource _scoreChangeAudio;

        private int _score;

        public void AddScore(int score)
        {
            _score += score;
            ScoreChanged?.Invoke(_score);
            _scoreChangeAudio.Play();
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetInt(GlobalConstants.SCORE_PREFS_KEY, _score);
            PlayerPrefs.Save();
        }
    }
}