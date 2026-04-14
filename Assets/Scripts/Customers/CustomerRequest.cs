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

        public List<string> requestString()
        {
            List<string> requestStrings = new List<string>();
            if (extraRules != null)
            {
                foreach (var rule in extraRules)
                {
                    requestStrings.Add(rule.RequestString);
                    Debug.Log(rule.RequestString);
                }
            }

            if (requiredItems != null)
            {
                foreach (var requiredItem in requiredItems)
                {
                    requestStrings.Add("Required: " + requiredItem.displayName);
                    Debug.Log("required");
                }
            }
            foreach(var s in requestStrings){
                Debug.Log("string " +  s);
            }
            return requestStrings;
          
        }
    }
}