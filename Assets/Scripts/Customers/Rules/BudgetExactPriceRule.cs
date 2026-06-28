using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/BudgetExactPrice")]
    public class BudgetExactPriceRule : RequestRule
    {
        private int pricePoint => setRandomPricePoint();

        public override string RequestString => $"Use exactly ${pricePoint}%";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            int totalCost = 0;
            foreach (var item in items)
                totalCost += item.itemData.cost;

            return pricePoint == totalCost;
        }

        public override string FailureString => "Not the right price!";
        public override float FailureDeduction => -0.3f;
        public override List<RequestTag> Tags => new List<RequestTag> { RequestTag.Efficient };

        private int setRandomPricePoint()
        {
            System.Random random = new System.Random();
            int randomPricePoint = random.Next(1, CustomerBudget);

            return randomPricePoint;
        }

    }
}
