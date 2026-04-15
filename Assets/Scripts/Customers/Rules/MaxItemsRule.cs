using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/MaxItemsRule")]
    public class MaxItemsRule : RequestRule
    {
        public int maxItems;

        public override string RequestString => "Items: Up to " + maxItems;

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            return items.Count <= maxItems;
        }



      
    }
}