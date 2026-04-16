using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/UniqueItemsRule")]

    public class UniqueItemsRule : RequestRule
    {
        public int minUnique;

        public override string RequestString => $"At least {minUnique} unique items";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            var unique = new HashSet<Item>();
            foreach (var item in items)
                unique.Add(item.itemData);

            return unique.Count >= minUnique;
        }

        public override string FailureString => "Not enough variety";
        public override float FailureDeduction => -0.3f;


    }

}