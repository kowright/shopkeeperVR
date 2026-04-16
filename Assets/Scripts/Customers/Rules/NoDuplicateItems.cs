using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/NoDuplicateItemsRule")]
    public class NoDuplicateItemsRule : RequestRule
    {
        public override string RequestString => "No duplicate items";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            var uniqueItems = new HashSet<Item>();

            foreach (var item in items)
            {
                if (!uniqueItems.Add(item.itemData))
                    return false;
            }

            return true;
        }

        public override string FailureString => "Duplicate items";
        public override float FailureDeduction => -0.3f;


    }
}