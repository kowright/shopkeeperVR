using Assets.Scripts.Customers;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "Scriptable Objects/Customer")]
public class Customer : ScriptableObject
{
    public string customerID;
    public string customerName;
    public int budget;
    public int patience;
    public CustomerType customerType;
    // public Item[] items; // use when customers can give stuff
    public List<CustomerRequest> possibleRequests;
    public CustomerRequest request;
    public int happiness; // tied to patience
    // something about conversation
}
