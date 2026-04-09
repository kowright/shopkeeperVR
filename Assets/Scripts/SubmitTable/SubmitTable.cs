using Assets.Scripts.Items;
using Assets.Scripts.SubmitTable;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class SubmitTable: MonoBehaviour
{
    [SerializeField] private List<ItemComponent> itemsOnTable = new List<ItemComponent>();
    public TextMeshProUGUI results;
    public CustomerTriggerZone customerZone;

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponentInParent<ItemComponent>();
        if (item != null)
        {
            itemsOnTable.Add(item);
            Debug.Log("Adding " + item.itemData.displayName);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var item = other.GetComponentInParent<ItemComponent>();
        Debug.Log($"Leaving {other.name}");
        if (item != null)
        {
            itemsOnTable.Remove(item);
            Debug.Log("removing " + item.itemData.displayName);
        }
    }

    public List<ItemComponent> GetItems() => itemsOnTable;

    public void SubmitItemsForValidation()
    {
        Debug.Log("SUBMIT" + itemsOnTable[0].itemData.displayName);
        string result; 
        bool submission = ValidateSubmission(itemsOnTable, customerZone.currentCustomer, out result);
        results.text = result;
        foreach (var item in itemsOnTable)
        {
           Destroy(item.gameObject);
            //maybe a sound or effect when destroyed
        }
     
        itemsOnTable.Clear();

        // process Customer happiness

        // process store profit!

    }

    public bool ValidateSubmission(List<ItemComponent> items, Customer customer, out string result)
    {
        var request = customerZone.currentCustomer.request;
        // 1. Required specific items
        if (request.requiredItems != null)
        {
            foreach (var reqItem in request.requiredItems)
            {
                if (!items.Exists(i => i.itemData == reqItem))
                {
                    result = $"Missing {reqItem.displayName}";
                    return false;
                }
            }
        }

        // 2. Required type
        if (request.requiredType.HasValue)
        {
            if (items.Exists(i => i.itemData.itemType != request.requiredType.Value))
            {
                result = $"All items must be {request.requiredType}";
                return false;
            }
        }

        // 3. Minimum quality
        if (request.minimumQuality.HasValue)
        {
            if (items.Exists(i => i.itemData.itemQuality < request.minimumQuality.Value))
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

