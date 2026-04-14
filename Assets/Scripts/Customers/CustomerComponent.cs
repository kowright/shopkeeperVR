using Assets.Scripts.Items;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomerComponent : MonoBehaviour
{
    public Customer customer;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI budgetText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI requestText;
    public TextMeshProUGUI patienceText;
    public TextMeshProUGUI itemQualityText;
    public TextMeshProUGUI itemTypeText;

    private void Start()
    {
        nameText.text = customer.customerName;
        budgetText.text = "$" + customer.budget.ToString();
        typeText.text = customer.customerType.ToString();

        itemQualityText.text = customer.request.minimumQuality.ToString();
        itemTypeText.text = customer.request.requiredType.ToString();

        //requestText.text = customer.request.requestString()[0]; //make new text mesh for each one?
        requestText.text = string.Join("\n", customer.request.requestString());


    }
}

