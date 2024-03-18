using System.Collections;
using Obstacle;
using Point;
using UI;
using Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] [Range(0.1f, 1)] private float _pointSpawnProbability = 0.7f;
        [SerializeField] private PointController _pointController;
        [SerializeField] private ObstacleController _obstacleController;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private LevelMover _levelMover;
        [SerializeField] private BackgroundColorController _backgroundColorController;
        [Tooltip("Points required to change background color")] [SerializeField] private int _colorChangePeriodInPoints = 5;
        [SerializeField] private int _difficultyIncreasePeriodInPoints = 10;
        [SerializeField] private float _sceneChangeDelay = 1.0f;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            _obstacleController.ObstacleChangePosition += OnObstacleChangePosition;
            _pointController.RewardAdded += _scoreController.AddScore;
            _playerController.PlayerDied += OnPlayerDied;
            _scoreController.ScoreChanged += OnScoreChanged;
        }

        private void OnObstacleChangePosition(Vector3 position)
        {
            var randomValue = Random.value;

            if (randomValue <= _pointSpawnProbability)
            {
                _pointController.SpawnPoint(position);
            }
        }

        private void OnScoreChanged(int score)
        {
            _scoreView.UpdateScoreLabel(score);

            if (score % _colorChangePeriodInPoints == 0)
            {
                _backgroundColorController.ChangeColor();
            }

            if (score % _difficultyIncreasePeriodInPoints == 0)
            {
                _levelMover.IncreaseSpeed();
            }
        }

        private void OnPlayerDied()
        {
            _levelMover.enabled = false;
            _obstacleController.DestroyAllObstacles();
            _pointController.DestroyAllPoints();

            StartCoroutine(LoadGameOverSceneWithDelay());
        }

        private IEnumerator LoadGameOverSceneWithDelay()
        {
            yield return new WaitForSeconds(_sceneChangeDelay);
            SceneManager.LoadSceneAsync(GlobalConstants.GAME_OVER_SCENE);
        }

        private void OnDestroy()
        {
            _obstacleController.ObstacleChangePosition -= OnObstacleChangePosition;
            _pointController.RewardAdded -= _scoreController.AddScore;
            _playerController.PlayerDied -= OnPlayerDied;
            _scoreController.ScoreChanged -= OnScoreChanged;
        }
    }
}