using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obstacle
{
    public class Obstacle : MonoBehaviour
    {
        public event Action PlayerPassedObstacle;

        [SerializeField] private float _minMoveY;
        [SerializeField] private float _maxMoveY;
        [SerializeField] private float _moveDuration;
        [SerializeField] private float _probabilityOfMoving = 0.3f;
        [SerializeField] private ObstacleMoveTrigger _obstacleMoveTrigger;
        [SerializeField] private ObstaclePassedTrigger _obstaclePassedTrigger;

        private SpriteRenderer _sprite;
        
        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();

            _obstacleMoveTrigger.PlayerEntered += MoveObstacleWithRandomChance;
            _obstaclePassedTrigger.PlayerPassedObstacle += OnPlayerPassedObstacle;
        }

        public void Initialize(float height)
        {
            var newSize = new Vector2(_sprite.size.x, height);
            _sprite.size = newSize;
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerController playerController))
            {
                playerController.DestroyPlayer();
            }
        }

        private void MoveObstacleWithRandomChance()
        {
            var randomChance = Random.value;

            if (randomChance <= _probabilityOfMoving)
            {
                var randomMoveY = Random.Range(_minMoveY, _maxMoveY);
                var nextPosition = transform.position.y + randomMoveY;
                transform.DOMoveY(nextPosition, _moveDuration);
            }
        }

        private void OnPlayerPassedObstacle()
        {
            PlayerPassedObstacle?.Invoke();
        }

        private void OnDestroy()
        {
            _obstacleMoveTrigger.PlayerEntered -= MoveObstacleWithRandomChance;
            _obstaclePassedTrigger.PlayerPassedObstacle -= PlayerPassedObstacle;
        }
    }
}