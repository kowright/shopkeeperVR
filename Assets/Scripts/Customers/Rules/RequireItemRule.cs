using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers
{
    /*
     * Define what every rule should be 
     */
    public abstract class RequestRule : ScriptableObject
    {
        public abstract bool IsSatisfied(List<Item> items, Customer customer);
    }
}