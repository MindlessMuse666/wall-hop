using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Unity
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreLabel;
        [SerializeField] private float _animationDuration = 0.2f;
        [SerializeField] private float _scaleFactor = 1.2f;

        private void Awake()
        {
            _scoreLabel.text = "0";
        }

        public void UpdateScoreLabel(int score)
        {
            _scoreLabel.text = score.ToString();

            _scoreLabel.transform
                .DOPunchScale(Vector3.one * _scaleFactor, _animationDuration, 0)
                .OnComplete(() => _scoreLabel.transform.localScale = Vector3.one);
        }
    }
}