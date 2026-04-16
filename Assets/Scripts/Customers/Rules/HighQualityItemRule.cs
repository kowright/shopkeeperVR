using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/HighQualityRequiredRule")]
    public class HighQualityRequiredRule : RequestRule
    {
        public ItemQuality requiredQuality;

        public override string RequestString => $"At least one {requiredQuality}+ item";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            return items.Exists(i => i.itemData.itemQuality >= requiredQuality);
        }

        public override string FailureString => "No high-quality item";
        public override float FailureDeduction => -0.3f;
    }
    
}