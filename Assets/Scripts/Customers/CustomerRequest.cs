using Assets.Scripts.Customers.Rules;
using Assets.Scripts.Items;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers
{

    [CreateAssetMenu(menuName = "Customer/Request")]
    public class CustomerRequest : ScriptableObject
    {
        [Header("Difficulty")]
        [Range(1, 10)]
        public int difficulty;

        // basic rules
        public List<Item> requiredItems;
    
        public bool hasRequiredType;
    
        [ShowIf(nameof(hasRequiredType))]
        public ItemType requiredType;

        public bool hasRequiredQuality;

        [ShowIf(nameof(hasRequiredQuality))]
        public ItemQuality minimumQuality;

        // custom rules from other scriptable objects
        public List<RequestRule> extraRules;

        public List<RequestTag> Tags;

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