using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "ColorProvider", menuName = "ColorProvider")]
    public class ColorProvider : ScriptableObject
    {
        public IReadOnlyList<Color> Colors => _colors;
        [field: SerializeField] public Color CurrentColor { get; set; }
        [SerializeField] List<Color> _colors;
    }
}