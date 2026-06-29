using Assets.Scripts.Customers;
using Assets.Scripts.Customers.Rules;
using Assets.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Requests/QualityRange")]
public class QualityRangeRule : RequestRule
{
    public ItemQuality minItemQuality;
    public ItemQuality maxItemQuality;

    public override string RequestString => $"All items have to be {minItemQuality} - {maxItemQuality}";

    public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
    {
        foreach (var item in items)
        {
            if (item.itemData.itemQuality >= maxItemQuality || item.itemData.itemQuality <= minItemQuality )
            {
                return false;
            }
        }
        return true;
    }

    public override string FailureString => "Not the right quality!";
    public override float FailureDeduction => -0.4f;
    public override List<RequestTag> Tags => new List<RequestTag> { };

}

