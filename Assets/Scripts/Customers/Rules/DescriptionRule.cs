using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.PackageManager.Requests;
using UnityEngine;


namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/DescriptionRule")]
    public class DescriptionRule : RequestRule
    {
        public List<Item> acceptableItems;
        public bool isAll;
        public string customRequestString;
        public string customFailureString;

        public override string RequestString => customRequestString;


        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            if (acceptableItems.Count == 0)
            {
                // other rules will determine acceptance
                return true; 
            }

            if (isAll) return items.All(i => acceptableItems.Contains(i.itemData));

            return items.Any(i => acceptableItems.Contains(i.itemData));
        }

        public override string FailureString => customRequestString;
        public override float FailureDeduction => -0.2f;
        public override List<RequestTag> Tags => new List<RequestTag> { };



    }

}
