using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Point
{
    public class PointController : MonoBehaviour
    {
        public event Action<int> RewardAdded;

        [SerializeField] private Point _pointPrefab;
        [SerializeField] private float _pointPositionY;
        [SerializeField] private int _rewardPerPoint = 1;

        private readonly List<Point> _points = new();

        private float _destroyPointDuration = 0.3f;

        public void SpawnPoint(Vector3 position)
        {
            var pointPosition = new Vector3(position.x, position.y);
            var point = Instantiate(_pointPrefab, pointPosition, Quaternion.identity, transform);

            point.Reward = _rewardPerPoint;

            point.PointCollected += OnPointCollected;
            point.PointMissed += OnPointMissed;

            _points.Add(point);
        }

        public void DestroyAllPoints()
        {
            foreach (var point in _points)
            {
                point.PointCollected -= OnPointCollected;

                point.transform
                    .DOScaleX(0f, _destroyPointDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => Destroy(point.gameObject));
            }

            _points.Clear();
        }

        private void OnPointCollected(Point point)
        {
            RewardAdded?.Invoke(point.Reward);

            point.PointCollected -= OnPointCollected;
            point.PointMissed -= OnPointMissed;

            _points.Remove(point);
            Destroy(point.gameObject);
        }

        private void OnPointMissed(Point point)
        {
            point.PointCollected -= OnPointCollected;
            point.PointMissed -= OnPointMissed;

            _points.Remove(point);
            Destroy(point.gameObject);
        }
    }
}