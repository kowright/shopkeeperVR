using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public enum ItemQuality
    {
        Bad = 0,
        Low = 1,
        Good = 2,
        Great = 3,
        Top = 4,
    }
    public class ItemOutlineColorManager //TODO: rename
    {
        private Dictionary<ItemQuality, Color> itemQualityOutlineColorMap = new Dictionary<ItemQuality, Color>()
        {
            { ItemQuality.Bad, Color.darkRed },
            { ItemQuality.Low, Color.red },
            { ItemQuality.Good, Color.lightGreen },
            { ItemQuality.Great, Color.green },
            { ItemQuality.Top, Color.magenta }
        };
        public Color GetOutlineColorForQuality(ItemQuality quality)
        {
            if (itemQualityOutlineColorMap.TryGetValue(quality, out Color color))
            {
                return color;
            }
            return Color.white;
        }
    }


}