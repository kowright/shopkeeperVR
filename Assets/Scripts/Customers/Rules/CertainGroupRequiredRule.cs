using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/CertainGroupRequiredRule")]
    public class CertainGroupRequiredRule : RequestRule
    {
        public ItemGroup requiredGroup;

        public override string RequestString => $"At least one {requiredGroup} item";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            return items.Any(i => i.itemData.itemGroup >= requiredGroup);
        }

        public override string FailureString => $"Nothing was a {requiredGroup}...";
        public override float FailureDeduction => -0.3f;

        public override List<RequestTag> Tags => new List<RequestTag> { RequestTag.Group };


    }

}