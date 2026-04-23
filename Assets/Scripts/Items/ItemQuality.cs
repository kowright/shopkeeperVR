using System.Collections.Generic;
using Unity.ProjectAuditor.Editor;
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
            { ItemQuality.Bad, Color.gray },
            { ItemQuality.Low,  Color.Lerp(Color.green, Color.darkGreen, 0.5f)},
            { ItemQuality.Good, Color.blue },
            { ItemQuality.Great, new Color(0.85f, 0.2f, 0.6f) },
            { ItemQuality.Top, new Color(1.0f, 0.78f, 0.2f) }
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