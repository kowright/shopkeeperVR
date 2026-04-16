using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/NoDuplicateTypeRule")]
    public class NoDuplicateTypeRule : RequestRule
    {
        public override string RequestString => "No duplicate item types";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            var types = new HashSet<ItemType>();

            foreach (var item in items)
            {
                if (!types.Add(item.itemData.itemType))
                    return false;
            }

            return true;
        }

        public override string FailureString => "Duplicate item types";
        public override float FailureDeduction => -0.3f;


    }
}