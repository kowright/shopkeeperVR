using Assets.Scripts.Customers.Rules;
using Assets.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers
{
    /*
     * Define what every rule should be 
     */
    public abstract class RequestRule : ScriptableObject
    {
        private int customerBudget;

        public int SetCustomerBudget
        {
            set { customerBudget = value; }
        }
        protected int CustomerBudget => customerBudget;

        public abstract bool IsSatisfied(List<ItemComponent> items, Customer customer);
        public abstract string RequestString { get; }

        public abstract string FailureString {get; }

        public abstract float FailureDeduction { get; }

        public abstract List<RequestTag> Tags { get; }
    }
}