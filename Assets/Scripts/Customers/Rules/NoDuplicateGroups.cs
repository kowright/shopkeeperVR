using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/NoDuplicateGroupsRule")]
    public class NoDuplicateGroupsRule : RequestRule
    {
        public override string RequestString => "No duplicate groups";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            var uniqueGroups = new HashSet<ItemGroup>();

            foreach (var item in items)
            {
                if (!uniqueGroups.Add(item.itemData.itemGroup))
                    return false;
            }

            return true;
        }

        public override string FailureString => "Duplicate groups!";
        public override float FailureDeduction => -0.3f;

        public override List<RequestTag> Tags => new List<RequestTag> { RequestTag.Variety, RequestTag.Group, RequestTag.MultipleItems };



    }
}
