using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SubmitTable: MonoBehaviour
{

    [SerializeField] private List<Item> itemsOnTable = new List<Item>();

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponentInParent<ItemComponent>();
        if (item != null)
        {
            itemsOnTable.Add(item.itemData);
            Debug.Log(item.itemData.displayName);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var item = other.GetComponent<ItemComponent>();
        if (item != null)
            itemsOnTable.Remove(item.itemData);
    }

    public List<Item> GetItems() => itemsOnTable;

    public void SubmitItemsForValidation()
    {
        Debug.Log("SUBMIT");
    }
}
