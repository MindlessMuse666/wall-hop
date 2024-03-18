using System;
using Game;
using UnityEngine;

namespace Obstacle
{
    public class ObstacleMoveTrigger : MonoBehaviour
    {
        public event Action PlayerEntered;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(GlobalConstants.PLAYER_TAG))
            {
                PlayerEntered?.Invoke();
            }
        }
    }
}