using Assets.Scripts.Customers;
using Assets.Scripts.Customers.Rules;
using Assets.Scripts.Items;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[CreateAssetMenu(menuName = "Requests/QualityOrLower")]
public class QualityOrLower : RequestRule
{
    public ItemQuality maxItemQuality;

    public override string RequestString => $"All items have to be {maxItemQuality} or lower";

    public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
    {
        foreach (var item in items)
        {
            if(item.itemData.itemQuality >= maxItemQuality)
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
