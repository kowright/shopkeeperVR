using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/SurpriseMeTypeRule")]
    public class SurpriseMeTypeRule : RequestRule
    {
        public ItemType preferredType;

        public override string RequestString => "Surprise me with a type of item!";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            return items.Exists(i => i.itemData.itemType == preferredType);
        }

        public override string FailureString => "Actually I don't like that one...";
        public override float FailureDeduction => -0.2f;


    }
}
