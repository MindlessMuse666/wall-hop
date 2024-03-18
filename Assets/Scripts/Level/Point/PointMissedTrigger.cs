using System;
using Game;
using UnityEngine;

namespace Point
{
    public class PointMissedTrigger : MonoBehaviour
    {
        public event Action PointMissed;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(GlobalConstants.PLAYER_TAG))
            {
                PointMissed?.Invoke();
            }
        }
    }
}