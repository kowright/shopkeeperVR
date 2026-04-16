using Assets.Scripts.Customers;
using Assets.Scripts.Customers.Rules;
using Assets.Scripts.Items;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.PackageManager.Requests;
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
    [SerializeField] private float happinessFloat;
    private Coroutine patienceCoroutine;
    private CustomerHappinessManager happinessManager;
    public RequestManager requestManager;
    public CustomerRequest request;

    //private void OnEnable()
    //{
    //    ProfitBoard.OnDayStarted += AssignRequest;
    //    Debug.Log("DAY STARTED sub");
    //    nameText.text = customer.customerName;
    //    budgetText.text = "$" + customer.budget.ToString();
    //    typeText.text = customer.customerType.ToString();
    //    Debug.Log("Request here?" + request.name);
    //    itemQualityText.text = request.minimumQuality.ToString();
    //    itemTypeText.text = request.requiredType.ToString();

    //    //requestText.text = customer.request.requestString()[0]; //make new text mesh for each one?
    //    requestText.text = string.Join("\n", request.requestString());
    //    Debug.Log("PATIENCE START AT" + customer.patience);
    //    patience = customer.patience;
    //    patienceText.text = "Patience " + customer.patience.ToString();
    //    happinessFloat = customer.happiness;
    //    happinessManager = new CustomerHappinessManager(customer.lowFineHappiness, customer.highFineHappiness);
    //    CustomerHappiness initialHappiness = happinessManager.GetHapinessFromPatience(customer.happiness);
    //    happinessText.text = initialHappiness.ToString();
    //}


    //private void OnDisable()
    //{

    //    ProfitBoard.OnDayStarted -= AssignRequest;
    //}
    private void Start()
    {

        Debug.Log("Customer getting request for day " + ProfitBoard.day);
        request = requestManager.GetRequest(customer, ProfitBoard.day);
        Debug.Log("GOT REQUEST" + request.name);

        nameText.text = customer.customerName;
        budgetText.text = "$" + customer.budget.ToString();
        typeText.text = customer.customerType.ToString();

        itemQualityText.text = request.minimumQuality.ToString();
        itemTypeText.text = request.requiredType.ToString();

        //requestText.text = customer.request.requestString()[0]; //make new text mesh for each one?
        requestText.text = string.Join("\n", request.requestString());
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
        patienceCoroutine = StartCoroutine(ReducePatience());
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
            happinessFloat = timeLeft;
            happinessText.text = happiness.ToString();

        }

        Debug.Log("Customer ran out of patience!");
    }

    public void ReduceHappiness(float reduction)
    {
        Debug.Log("REduce happiness by " +  reduction);
        happinessFloat += reduction;
        happiness = happinessManager.GetHapinessFromPatience(happinessFloat);
        Debug.Log("happiness" + happinessFloat);
        Debug.Log("official happiness" + happiness.ToString());
        happinessText.text = happiness.ToString();

    }

    public void StopPatienceTimer()
    {
        if (patienceCoroutine != null)
        {
            StopCoroutine(patienceCoroutine);
            patienceCoroutine = null;
        }
    }

    private void AssignRequest(int day)
    {
        Debug.Log("Customer getting request for day " +  day);
        request = requestManager.GetRequest(customer, ProfitBoard.day);
        Debug.Log("GOT REQUEST" + request.name);
    }

}