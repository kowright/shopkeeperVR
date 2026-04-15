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
    public float happiness; // starting patience; out of highest is 1.0
    public float lowFineHappiness;
    public float highFineHappiness;
    // something about conversation

}

