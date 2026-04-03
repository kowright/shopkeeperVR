using UnityEngine;
using System.Collections;
using Assets.Scripts.Items;
using NUnit.Framework;
using System.Collections.Generic;

namespace Assets.Scripts.Customers
{
    [CreateAssetMenu(menuName = "Customer/Request")]
    public class CustomerRequest : ScriptableObject
    {
        public List<Item> requiredItems;
        public ItemType? requiredType;
        public ItemQuality? minimumQuality;

        public List<RequestRule> extraRules;
    }
}