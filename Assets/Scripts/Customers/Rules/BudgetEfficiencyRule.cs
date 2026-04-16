using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    public class BudgetEfficiencyRule : RequestRule
    {
        public float maxBudgetUsage = 0.8f;

        public override string RequestString => $"Use under {(maxBudgetUsage * 100)}% of budget";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            int totalCost = 0;
            foreach (var item in items)
                totalCost += item.itemData.cost;

            return totalCost <= customer.budget * maxBudgetUsage;
        }

        public override string FailureString => "Too expensive!";
        public override float FailureDeduction => -0.3f;
    }
}