using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/SpendMyBudgetRule")]
    public class SpendMyBudgetRule : RequestRule
    {
        private int totalItemCount;
        private int difference;

        public override string RequestString => "Take all my money!";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            foreach (ItemComponent item in items)
            {
                totalItemCount += item.itemData.cost;
            }
            difference = totalItemCount - customer.budget;
            return totalItemCount == customer.budget;
            
        }

        public override string FailureString =>
            $"Submitted items that were {difference} from the budget";
        public override float FailureDeduction => -0.2f;



    }
}