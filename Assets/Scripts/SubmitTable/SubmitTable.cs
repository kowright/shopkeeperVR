using Assets.Scripts.Items;
using Assets.Scripts.SubmitTable;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class SubmitTable: MonoBehaviour
{

    [SerializeField] private List<Item> itemsOnTable = new List<Item>();
    public TextMeshProUGUI results;
    public CustomerTriggerZone customerZone;

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponentInParent<ItemComponent>();
        if (item != null)
        {
            itemsOnTable.Add(item.itemData);
            Debug.Log("Adding " + item.itemData.displayName);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var item = other.GetComponentInParent<ItemComponent>();
        Debug.Log($"Leaving {other.name}");
        if (item != null)
        {
            itemsOnTable.Remove(item.itemData);
            Debug.Log("removing " + item.itemData.displayName);
        }
    }

    public List<Item> GetItems() => itemsOnTable;

    public void SubmitItemsForValidation()
    {
        Debug.Log("SUBMIT");
        string result; 
        ValidateSubmission(itemsOnTable, customerZone.currentCustomer, out result);
        results.text = result;


        //if (customerZone.currentCustomer.request.requiredItems != null)
        //{
        //    foreach (Item item in customerZone.currentCustomer.request.requiredItems)
        //    {
        //        Debug.Log("requested " + customerZone.currentCustomer.request.requiredItems[0].displayName);
        //        Debug.Log("table" + item.displayName);
        //        if (!itemsOnTable.Find(i => i == item))
        //        {
        //            result = "THERE IS NO " + item.displayName;
        //            results.text = result;
        //            return;
        //        }

        //    }
        //    Debug.Log("no return");
        //    result = "GOOD JOB";
        //    results.text = result;
        //}
        

       
    }

    public bool ValidateSubmission(List<Item> items, Customer customer, out string result)
    {
        var request = customerZone.currentCustomer.request;
        // 1. Required specific items
        if (request.requiredItems != null)
        {
            foreach (var reqItem in request.requiredItems)
            {
                if (!items.Exists(i => i == reqItem))
                {
                    result = $"Missing {reqItem.displayName}";
                    return false;
                }
            }
        }

        // 2. Required type
        if (request.requiredType.HasValue)
        {
            if (items.Exists(i => i.itemType != request.requiredType.Value))
            {
                result = $"All items must be {request.requiredType}";
                return false;
            }
        }

        // 3. Minimum quality
        if (request.minimumQuality.HasValue)
        {
            if (items.Exists(i => i.itemQuality < request.minimumQuality.Value))
            {
                result = $"All items must be at least {request.minimumQuality}";
                return false;
            }
        }

        // 4. Extra rules (modular system)
        if (request.extraRules != null)
        {
            foreach (var rule in request.extraRules)
            {
                if (!rule.IsSatisfied(items, customer))
                {
                    result = $"Failed rule: {rule.name}";
                    return false;
                }
            }
        }

        result = "Success!";
        return true;
    }
}

