using System;
using Game;
using UnityEngine;

namespace Obstacle
{
    public class ObstaclePassedTrigger : MonoBehaviour
    {
        public event Action PlayerPassedObstacle;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(GlobalConstants.PLAYER_TAG))
            {
                PlayerPassedObstacle?.Invoke();
            }
        }
    }
}