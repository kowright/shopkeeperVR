using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Customers.Rules
{
    public enum RequestTag
    {
        Cheap,
        MidRange, // personality Average
        HighRange, // personality Luxury
        /// <summary>High spending customer. Equals CustomerType BigSpender.</summary>
        Expensive,
        Fast,
        Slow,
        Combo,
        HighQuality,
        MidQuality, // personality Practical
        Risky,
        MultipleItems,
        SingleItem,
        /// <summary>Prefers a variety of unique items, whether variety in type or group etc.  Equals CustomerType Diverse.</summary>
        Variety, // personality Diverse
        Group,
        /// <summary> Prefers to have their budget standard followed. Equals CustomerType Budgeter. </summary>
        Efficient,
    }
}