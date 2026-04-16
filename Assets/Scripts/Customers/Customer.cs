using Assets.Scripts.Customers;
using NaughtyAttributes;
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
    //public CustomerRequest request;
    public float happiness; // starting patience; out of highest is 1.0

    // something about conversation

    [MinMaxSlider(0.0f, 1.0f)]
    public Vector2 minMaxFineHappinessSlider = new Vector2(0.25f, 0.75f);

    public float lowFineHappiness => minMaxFineHappinessSlider.x;
    public float highFineHappiness => minMaxFineHappinessSlider.y;


}

