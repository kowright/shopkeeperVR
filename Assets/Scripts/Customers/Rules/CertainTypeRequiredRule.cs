using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{

    [CreateAssetMenu(menuName = "Requests/CertainTypeRequiredRule")]
    public class CertainTypeRequiredRule : RequestRule
    {
        public ItemType requiredType;

        public override string RequestString => $"At least one {requiredType} item";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            return items.Any(i => i.itemData.itemType >= requiredType);
        }

        public override string FailureString => $"Nothing was a {requiredType}...";
        public override float FailureDeduction => -0.3f;

        public override List<RequestTag> Tags => new List<RequestTag> { RequestTag.Group };


    }


}
