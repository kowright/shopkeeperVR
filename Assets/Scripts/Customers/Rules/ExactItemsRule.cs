using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/ExactItemsRule")]
    public class ExactItemsRule : RequestRule
    {
        public int requiredItems;

        public override string RequestString => "Items: Must have " + requiredItems + $" item{(requiredItems == 1 ? "" : "s")} ";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            return items.Count >= requiredItems;
        }

        public override string FailureString =>
            $"Different amount than {requiredItems} item{(requiredItems == 1 ? "" : "s")} {(requiredItems == 1 ? "was" : "were")} submitted";
        public override float FailureDeduction => -0.3f;


        public override List<RequestTag> Tags => new List<RequestTag> { generateRequestTags() };

        private RequestTag generateRequestTags()
        {
            return requiredItems > 1 ? RequestTag.MultipleItems : RequestTag.SingleItem;
        }

    }
}