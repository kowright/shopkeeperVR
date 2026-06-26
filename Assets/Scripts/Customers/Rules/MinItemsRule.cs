using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/MinItemsRule")]
    public class MinItemsRule : RequestRule
    {
        public int minItems;

        public override string RequestString => "Items: At least " + minItems + " items";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            return items.Count >= minItems;
        }

        public override string FailureString =>
            $"Less than {minItems} item{(minItems == 1 ? "" : "s")} {(minItems == 1 ? "was" : "were")} submitted";
        public override float FailureDeduction => -0.3f;

        public override List<RequestTag> Tags => new List<RequestTag> { generateRequestTags() };

        private RequestTag generateRequestTags()
        {
            return minItems > 1 ? RequestTag.MultipleItems : RequestTag.SingleItem;
        }


    }
}