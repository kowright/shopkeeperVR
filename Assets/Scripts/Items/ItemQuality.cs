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
            { ItemQuality.Bad, Color.indianRed },
            { ItemQuality.Low, Color.orange },
            { ItemQuality.Good, Color.green },
            { ItemQuality.Great, Color.blue },
            { ItemQuality.Top, Color.deepPink }
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

    public class ItemRespawnManager
    {
        private Dictionary<ItemQuality, float> itemQualityRespawnTimeMap = new Dictionary<ItemQuality, float>()
        {
            { ItemQuality.Bad, 1},
            { ItemQuality.Low, 3 },
            { ItemQuality.Good, 5},
            { ItemQuality.Great, 10 },
            { ItemQuality.Top, 60 }
        };
        public float GetRespawnTimeForQuality(ItemQuality quality)
        {
            if (itemQualityRespawnTimeMap.TryGetValue(quality, out float time))
            {
                return time;
            }
            return 0;
        }
    }

}