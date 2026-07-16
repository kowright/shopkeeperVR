using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    namespace Assets.Scripts.Customers.Rules
    {
        [CreateAssetMenu(menuName = "Requests/NoSpecificGroupRule")]
        public class NoSpecificGroupRule : RequestRule
        {
            public ItemGroup bannedGroup;

            public override string RequestString => $"No {bannedGroup} items";

            public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
            {
                return items.Any(i => i.itemData.itemGroup == bannedGroup);
            }

            public override string FailureString => $"{bannedGroup} was included";
            public override float FailureDeduction => -0.4f;

            public override List<RequestTag> Tags => new List<RequestTag> { RequestTag.Group };
        }
    }

}
