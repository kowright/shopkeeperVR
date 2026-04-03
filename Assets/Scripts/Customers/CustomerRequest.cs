using UnityEngine;
using System.Collections;
using Assets.Scripts.Items;
using System.Collections.Generic;

namespace Assets.Scripts.Customers
{
    [CreateAssetMenu(menuName = "Customer/Request")]
    public class CustomerRequest : ScriptableObject
    {
        // basic rules
        public List<Item> requiredItems;
        public ItemType? requiredType;
        public ItemQuality? minimumQuality;

        // custom rules from other scriptable objects
        public List<RequestRule> extraRules;
    }
}