using Assets.Scripts.Customers;
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
    public TextMeshProUGUI happinessText;
    public TextMeshProUGUI patienceText;
    public TextMeshProUGUI itemQualityText;
    public TextMeshProUGUI itemTypeText;
    private float patience;
    private CustomerHappiness happiness;
    private float happinessFloat;
    private CustomerHappinessManager happinessManager;

    private void Start()
    {
        nameText.text = customer.customerName;
        budgetText.text = "$" + customer.budget.ToString();
        typeText.text = customer.customerType.ToString();

        itemQualityText.text = customer.request.minimumQuality.ToString();
        itemTypeText.text = customer.request.requiredType.ToString();

        //requestText.text = customer.request.requestString()[0]; //make new text mesh for each one?
        requestText.text = string.Join("\n", customer.request.requestString());
        Debug.Log("PATIENCE START AT" + customer.patience);
        patience = customer.patience;
        patienceText.text = "Patience " + customer.patience.ToString();
        happinessFloat = customer.happiness;
        happinessManager = new CustomerHappinessManager(customer.lowFineHappiness, customer.highFineHappiness);
        CustomerHappiness initialHappiness = happinessManager.GetHapinessFromPatience(customer.happiness);
        happinessText.text = initialHappiness.ToString();

    }

    public void StartPatienceTimer()
    {
        StartCoroutine(ReducePatience());
    }

    private System.Collections.IEnumerator ReducePatience()
    {
        while (patience > 0)
        {
            yield return new WaitForSeconds(1f);

            patience -= 1;

            patienceText.text = "Patience " + patience.ToString();

            float timeLeft = patience / customer.patience;
            happiness = happinessManager.GetHapinessFromPatience(timeLeft);

            happinessText.text = happiness.ToString();
         
        }

        Debug.Log("Customer ran out of patience!");

    }
}

