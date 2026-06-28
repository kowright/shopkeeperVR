using Assets.Scripts.Customers.Rules;
using Assets.Scripts.Items;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
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
        public ItemType requiredType = ItemType.None;


        public bool hasRequiredQuality;

        [ShowIf(nameof(hasRequiredQuality))]
        public ItemQuality minimumQuality = ItemQuality.None;

        // custom rules from other scriptable objects
        public List<RequestRule> extraRules;

        public List<RequestTag> Tags;

        [ReadOnly]
        public List<RequestTag> suggestedTags;

        private RequestTag costTag = RequestTag.Cheap;

        [ShowNativeProperty]
        public float totalCostOfRequiredItems => costOfRequiredItems();

        private int _customerBudget;
        public int customerBudget => _customerBudget;

        public void Initialize(Customer customer)
        {
            _customerBudget = customer.budget;
            foreach(var rule in extraRules)
            {
                rule.SetCustomerBudget = customer.budget;
            }
        }

        private void OnValidate()
        {
            suggestedTags = tagsFromRulesAndCost();
            //if (costTag != null)
            //{
            //    suggestedTags.Add((RequestTag)_costTag);
            //}
        }

        private int costOfRequiredItems()
        {
            int totalCost = 0;

            foreach (var item in requiredItems)
            {
                totalCost += item.cost;
            }

            if (totalCost != 0)
            {

                if (totalCost <= 20)
                {
                    costTag = RequestTag.Cheap;
                }
                else if (totalCost <= 100)
                {
                    costTag = RequestTag.MidRange;
                }
                else if (totalCost <= 500)
                {
                    costTag = RequestTag.HighRange;
                }
                else
                {
                    costTag = RequestTag.Expensive;
                }
            }

            return totalCost;
        }

        private List<RequestTag> tagsFromRulesAndCost()
        {
            List<RequestTag> tags = new List<RequestTag>();
   
            foreach (var rule in extraRules)
            {
                if (rule.Tags.Count > 0)
                {
                    foreach (var tag in rule.Tags)
                    {
                        if (!tags.Contains(tag))
                        {
                            tags.Add(tag);
                        }
                   
                    }
                }
            }
            
            // cost tag
            if (costOfRequiredItems() != 0)
            {
                if (!tags.Contains(costTag))
                    tags.Add(costTag);
            }

            // quality tag
            if (hasRequiredQuality)
            {
                RequestTag? qualityTag = ItemUtils.GetRequestTagForItemQuality(minimumQuality);
                if(qualityTag != null)
                {
        
                    if (!tags.Contains(qualityTag.Value)){
                        tags.Add(qualityTag.Value);
                    }
                }
            }
            //    if (minimumQuality == ItemQuality.Great || minimumQuality == ItemQuality.Great)
            //    {
            //        if (!tags.Contains(RequestTag.HighQuality))
            //            tags.Add(RequestTag.HighQuality);
            //    }
            //    else if (minimumQuality == ItemQuality.Good)
            //    {
            //        if (!tags.Contains(RequestTag.MidQuality))
            //            tags.Add(RequestTag.MidQuality);
            //    }

            //}

            return tags;
        }


        public List<string> requestString()
        {
            List<string> requestStrings = new List<string>();

            //TODO: should there be request strings for quality? like the customer has a minimum quality on their customer component, 
            // but they have no required quality- should the user be rewarded if they give above the quality even though it isn't required?
            if (extraRules != null)
            {
                foreach (var rule in extraRules)
                {
                    requestStrings.Add(rule.RequestString);
                    //Debug.Log("Request rule string: " + rule.RequestString);
                }
            }

            if (requiredItems != null)
            {
                foreach (var requiredItem in requiredItems)
                {
                    requestStrings.Add("Required: " + requiredItem.displayName);
                    //Debug.Log("required");
                }
            }
            foreach(var s in requestStrings){
                //Debug.Log("string " +  s);
            }

            Debug.Log("Request has " + requestStrings.Count + " strings that are: " + string.Join("\n", requestStrings));
            return requestStrings;
          
        }

    }
}