using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/NoSpecificTypeRule")]
    public class NoSpecificTypeRule : RequestRule
    {
        public ItemType bannedType;

        public override string RequestString => $"No {bannedType} items";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            return items.Any(i => i.itemData.itemType == bannedType);
        }

        public override string FailureString => $"{bannedType} was included";
        public override float FailureDeduction => -0.4f;

        public override List<RequestTag> Tags => new List<RequestTag> { };
    }
}
