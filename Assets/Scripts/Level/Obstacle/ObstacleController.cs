using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obstacle
{
    public class ObstacleController : MonoBehaviour
    {
        public event Action<Vector3> ObstacleChangePosition;

        private const int COUNT_OF_OBSTACLES = 10;

        [SerializeField] private Obstacle _obstaclePrefab;
        [SerializeField] private float _minDistanceBetweenObstaclesX = 5.0f; 
        [SerializeField] private float _maxDistanceBetweenObstaclesX = 9.0f;
        [SerializeField] private float _minObstacleHeight = 1.2f;
        [SerializeField] private float _maxObstacleHeight = 1.6f;
        [SerializeField] private float _minObstaclePositionY = -0.5f;
        [SerializeField] private float _maxObstaclePositionY = -2.5f;
        [SerializeField] private float _destroyObstacleDuration = 0.3f;

        private readonly Queue<Obstacle> _obstacles = new();

        private void Start()
        {
            SpawnInitialObstacles();
        } 

        public void DestroyAllObstacles()
        {
            while (_obstacles.Count > 0)
            {
                var obstacle = _obstacles.Dequeue();
                obstacle.PlayerPassedObstacle -= OnPlayerPassed;

                obstacle.transform
                    .DOScaleX(0f, _destroyObstacleDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => Destroy(obstacle.gameObject));
            }
        }

        private void SpawnInitialObstacles()
        {
            for (var i = 0; i < COUNT_OF_OBSTACLES; i++)
            {
                SpawnObstacle();
            }
        }

        private void InitializeObstacle(Obstacle obstacle)
        {
            var obstacleRandomHeight = Random.Range(_minObstacleHeight, _maxObstacleHeight);
            obstacle.Initialize(obstacleRandomHeight);
            obstacle.PlayerPassedObstacle += OnPlayerPassed;
        }

        private void SpawnObstacle()
        {
            var obstacle = Instantiate(_obstaclePrefab, transform);

            InitializeObstacle(obstacle);
            SetObstaclePosition(obstacle);
            
            _obstacles.Enqueue(obstacle);
        }

        private void SetObstaclePosition(Obstacle obstacle)
        {
            var previousObstacle = _obstacles.Count > 0 ? _obstacles.Last() : null;
            var previousPosition = GetPreviousPosition(previousObstacle);
            var nextRandomPosition = GetNextRandomPosition(previousPosition);

            obstacle.transform.position = nextRandomPosition; 

            if (_obstacles.Count > 1)
            {
                var positionBetweenObstacle = (previousPosition + obstacle.transform.position) / 2;

                ObstacleChangePosition?.Invoke(positionBetweenObstacle);
            }
        }

        private Vector3 GetPreviousPosition(Obstacle previousObstacle)
        {
            Vector3 previousPosition;

            if (previousObstacle == null)
            {
                var randomPositionX = Random.Range(_minDistanceBetweenObstaclesX, _maxDistanceBetweenObstaclesX); 
                var randomPositionY = Random.Range(_minObstaclePositionY, _maxObstaclePositionY);

                previousPosition = new Vector3(randomPositionX, randomPositionY);
            }
            else
            {
                previousPosition = previousObstacle.transform.position;
            }

            return previousPosition;
        }

        private Vector3 GetNextRandomPosition(Vector3 previousPosition)
        {
            var randomSpawnPositionX = Random.Range(_minDistanceBetweenObstaclesX, _maxDistanceBetweenObstaclesX);
            var randomSpawnPositionY = Random.Range(_minObstaclePositionY, _maxObstaclePositionY);

            var spawnPosition = new Vector3(randomSpawnPositionX + previousPosition.x, randomSpawnPositionY, previousPosition.z);
            return spawnPosition;
        }

        private void MoveObstacleToEndTrack()
        {
            var obstacle = _obstacles.Dequeue();
            SetObstaclePosition(obstacle);
            _obstacles.Enqueue(obstacle);
        }

        private void OnPlayerPassed()
        {
            MoveObstacleToEndTrack();
        }

        private void OnDestroy()
        {
            foreach(var obstacle in _obstacles)
            {
                obstacle.PlayerPassedObstacle -= OnPlayerPassed;
            }
        }
    }
}