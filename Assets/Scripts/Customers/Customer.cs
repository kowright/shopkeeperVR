using Assets.Scripts.Customers;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class Customer
{
    public string customerName;
    public int budget;
    public int patience;
    public List<CustomerType> customerTypes;
    public float happiness;
    public float lowFineHappiness;
    public float highFineHappiness;

    // something about conversation later
    // somethign about customers giving you stuff maybe

    public Customer(int budget, int patience, List<CustomerType> customerTypes, float happiness, float lowFineHappiness, float highFineHappiness )
    {
        this.budget = budget;
        this.patience = patience;
        this.customerTypes = customerTypes ?? new List<CustomerType>();
        this.happiness = happiness;
        this.lowFineHappiness = lowFineHappiness;
        this.highFineHappiness = highFineHappiness;
        this.customerName = NamesRegistry.GetRandomName();
    }
}