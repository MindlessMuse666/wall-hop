using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class BackgroundColorController : MonoBehaviour
    {
        [SerializeField] private ColorProvider _colorProvider;
        [SerializeField] private float _colorChangeDuration;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;

            Camera.main.backgroundColor = _colorProvider.CurrentColor;
        }

        public void ChangeColor()
        {
            var nextColor = _colorProvider.GetRandomColor(except: _colorProvider.CurrentColor);

            _camera
                .DOColor(nextColor, _colorChangeDuration)
                .SetEase(Ease.OutFlash);

            _colorProvider.CurrentColor = nextColor;
        }
    }
}