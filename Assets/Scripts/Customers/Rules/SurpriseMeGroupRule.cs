using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/SurpriseMeGroupRule")]
    public class SurpriseMeGroupRule : RequestRule
    {
        public ItemGroup preferredGroup;

        public override string RequestString => "Surprise me with a group of item!";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            return items.Exists(i => i.itemData.itemGroup == preferredGroup);
        }

        public override string FailureString => "Actually I don't like that one...";
        public override float FailureDeduction => -0.2f;


    }
}
