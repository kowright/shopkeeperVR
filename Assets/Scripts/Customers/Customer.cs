using Assets.Scripts.Customers;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class Customer
{
    public string customerName = "John"; //TODO change later
    public int budget;
    public int patience;
    public List<CustomerType> customerTypes;
    public float happiness;
    public float lowFineHappiness;
    public float highFineHappiness;

    // TODO name generator

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
    }
}


//[CreateAssetMenu(fileName = "Customer", menuName = "Scriptable Objects/Customer")]
//public class Customer : ScriptableObject
//{
//    public string customerID;
//    public string customerName;
//    public int budget;
//    public int patience;
//    public List<CustomerType> customerTypes;
//    // public Item[] items; // use when customers can give stuff
//    public float happiness; // starting patience; out of highest is 1.0

//    // something about conversation

//    [MinMaxSlider(0.0f, 1.0f)]
//    public Vector2 minMaxFineHappinessSlider = new Vector2(0.25f, 0.75f);

//    public float lowFineHappiness => minMaxFineHappinessSlider.x;
//    public float highFineHappiness => minMaxFineHappinessSlider.y;


//}

