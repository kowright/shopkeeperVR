using Assets.Scripts.Customers;
using Assets.Scripts.Customers.Rules;
using Assets.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Requests/BudgetPercentageRangeRule")]
public class BudgetPercentageRangeRule : RequestRule
{
    public float maxBudgetUsage = 0.8f;
    public float minBudgetUsage = 0.3f;

    public override string RequestString => $"Use between {minBudgetUsage * 100}% & {maxBudgetUsage * 100}%";

    public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
    {
        int totalCost = 0;
        foreach (var item in items)
            totalCost += item.itemData.cost;

        return totalCost <= customer.budget * maxBudgetUsage && totalCost >= customer.budget * minBudgetUsage;
    }

    public override string FailureString => "Not the right price!";
    public override float FailureDeduction => -0.3f;
    public override List<RequestTag> Tags => new List<RequestTag> { RequestTag.Efficient };
}

    
