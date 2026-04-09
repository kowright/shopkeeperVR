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
        public abstract bool IsSatisfied(List<ItemComponent> items, Customer customer);
    }
}