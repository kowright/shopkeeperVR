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
        var result = "YES";
        if (customerZone.currentCustomer.request.requiredItems != null)
        {
            foreach (Item item in customerZone.currentCustomer.request.requiredItems)
            {
                Debug.Log("requested " + customerZone.currentCustomer.request.requiredItems[0].displayName);
                Debug.Log("table" + item.displayName);
                if (!itemsOnTable.Find(i => i.displayName == item.displayName))
                {
                    result = "THERE IS NO " + item.displayName;
                    results.text = result;
                    return;
                }

            }
            Debug.Log("no return");
            result = "GOOD JOB";
            results.text = result;
        }
        

       
    }
}
