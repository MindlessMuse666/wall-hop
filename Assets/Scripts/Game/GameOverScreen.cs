using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private ColorProvider _colorProvider;
        [SerializeField] private TextMeshProUGUI _currenScoreLabel;
        [SerializeField] private TextMeshProUGUI _bestScoreLabel;
        [SerializeField] private float _newBestScoreAnimationDuration = 0.3f;
        [SerializeField] private AudioSource _bestScoreChangedAudio;

        private void Awake()
        {
            Camera.main.backgroundColor = _colorProvider.CurrentColor;

            var currentScore = PlayerPrefs.GetInt(GlobalConstants.SCORE_PREFS_KEY);
            var bestScore = PlayerPrefs.GetInt(GlobalConstants.BEST_SCORE_PREFS_KEY);

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                ShowNewBestScoreAnimation();
                SaveNewBestScore(bestScore);
            }

            _currenScoreLabel.text = currentScore.ToString();
            _bestScoreLabel.text = $"BEST {bestScore.ToString()}";
        }

        public void RestartGame()
        {
            _colorProvider.CurrentColor = _colorProvider.GetRandomColor(_colorProvider.CurrentColor);
            SceneManager.LoadSceneAsync(GlobalConstants.GAME_SCENE);    
        }

        public void ExitGame()
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
        }

        private void ShowNewBestScoreAnimation()
        {
            _bestScoreChangedAudio.transform.DOPunchScale(Vector3.one, _newBestScoreAnimationDuration, 0);
            _bestScoreChangedAudio.Play();
        }

        private void SaveNewBestScore(int bestScore)
        {
            PlayerPrefs.SetInt(GlobalConstants.BEST_SCORE_PREFS_KEY, bestScore);
            PlayerPrefs.Save();
        }
    }
}