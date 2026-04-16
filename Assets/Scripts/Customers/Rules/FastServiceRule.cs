using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Requests/FastServiceRule")]
    public class FastServiceRule: RequestRule
	{
        public float requiredPatiencePercent = 0.5f;

        public override string RequestString => "Serve me quickly!";

        public override bool IsSatisfied(List<ItemComponent> items, Customer customer)
        {
            return customer.patience >= requiredPatiencePercent;
        }

        public override string FailureString => "Too slow!";
        public override float FailureDeduction => -0.4f;
        
    }
}