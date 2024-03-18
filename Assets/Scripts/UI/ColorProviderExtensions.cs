using System.Linq;
using UnityEngine;

namespace UI
{
    public static class ColorProviderExtensions
    {
        public static Color GetRandomColor(this ColorProvider colorProvider, Color except)
        {
            var availableColors = colorProvider.Colors
                .Where(color => color != except)
                .ToList();
            
            var randomIndex = Random.Range(0, availableColors.Count);

            return availableColors[randomIndex];
        } 

        public static Color GetRandomColor(this ColorProvider colorProvider)
        {
            var randomIndex = Random.Range(0, colorProvider.Colors.Count);
            var color = colorProvider.Colors[randomIndex];
            
            return color;
        }
    }
}