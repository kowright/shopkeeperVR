using Assets.Scripts.Items;
using Assets.Scripts.SubmitTable;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.PackageManager.Requests;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class SubmitTable: MonoBehaviour
{
    [SerializeField] private List<ItemComponent> itemsOnTable = new List<ItemComponent>();
    public TextMeshProUGUI results;
    public CustomerTriggerZone customerZone;
    private int tableRevenue; // how much table has made today
    public static Action<int> OnTableSubmitted;

    void Start()
    {

    }

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
        customerZone.currentCustomerComponent.StopPatienceTimer();
        var ( result, happinessReduction) = ValidateSubmission(itemsOnTable, customerZone.currentCustomer);
        if (result.Count > 0)
        {
            results.text = result[0];
        }
        foreach (var item in itemsOnTable)
        {
           Destroy(item.gameObject);
            //maybe a sound or effect when destroyed
        }
     
        itemsOnTable.Clear();

        customerZone.currentCustomerComponent.ReduceHappiness(happinessReduction);



        // process store profit!

    }

    public (List<string> result, float happinessReduction)  ValidateSubmission(List<ItemComponent> items, Customer customer)
    {
        // can reward for how much money left the person has
        // ALLOW MISTAKES TO GO THROUGH

       

        // 0. Cost
        int totalCost = 0;
        foreach (var item in items)
        {
            totalCost += item.itemData.cost;
        }
        int customerMoneyLeft = customer.budget - totalCost;
        Debug.Log("customer budget is now " + customerMoneyLeft);
        List<string> results = new();
        if (customerMoneyLeft < 0)
        {
            string result = $"Overbudget by -{customerMoneyLeft}";
            results.Add(result);
            return (results, -1.0f);
            
        }
        tableRevenue += totalCost;
        OnTableSubmitted?.Invoke(totalCost);
        Debug.Log("Submit table check rules");
    
        float happinessReduction = 0f;

        var request = customerZone.currentCustomer.request;
        // 1. Required specific items
        if (request.requiredItems != null)
        {
            foreach (var reqItem in request.requiredItems)
            {
                if (!items.Exists(i => i.itemData == reqItem))
                {
                    //string result = $"Missing {reqItem.displayName}";

                    //return (result, -0.5f);
                    results.Add($"Missing {reqItem.displayName}");
                    happinessReduction += -0.5f;
                }
            }
        }

        // 2. Required type
        if (request.hasRequiredType)
        {
            if (items.Exists(i => i.itemData.itemType != request.requiredType))
            {
                //string result = $"All items must be {request.requiredType}";
                //return (result, -0.2f);
                results.Add($"All items must be {request.requiredType}");
                happinessReduction += -0.2f;
            }
        }

        // 3. Minimum quality
        if (request.hasRequiredQuality)
        {
            if (items.Exists(i => i.itemData.itemQuality < request.minimumQuality))
            {
                //string result = $"All items must be at least {request.minimumQuality}";
                //return (result, -0.2f);
                results.Add($"All items must be at least {request.minimumQuality}");
                happinessReduction += -0.2f;
            }
        }

        // 4. Extra rules (modular system)
        if (request.extraRules != null)
        {
            foreach (var rule in request.extraRules)
            {
                if (!rule.IsSatisfied(items, customer))
                {
                    //string result = $"Failed rule: {rule.name}";
                    //return (result, -0.2f);
                    results.Add(rule.FailureString);
                    happinessReduction += rule.FailureDeduction;
                }
            }
        }

        return (results, happinessReduction);
    }
}

