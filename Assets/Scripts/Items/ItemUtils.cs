using Assets.Scripts.Customers.Rules;
using Assets.Scripts.Items;
using UnityEngine;

public static class ItemUtils
{
    public static RequestTag? GetRequestTagForItemQuality(ItemQuality itemQuality)
    {
        if (itemQuality == ItemQuality.Great || itemQuality == ItemQuality.Great)
        {
            return RequestTag.HighQuality;
        }
        else if (itemQuality == ItemQuality.Good)
        {
            return RequestTag.MidQuality;
        }

        return null;
    }
}
