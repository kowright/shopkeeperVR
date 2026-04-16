using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/SurpriseMeTypeRule")]
    public class SurpriseMeTypeRule : RequestRule
    {
        public ItemType preferredType;

        public override string RequestString => "Surprise me!";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            return items.Exists(i => i.itemData.itemType == preferredType);
        }

        public override string FailureString => "I didn't like that...";
        public override float FailureDeduction => -0.2f;
    }

}