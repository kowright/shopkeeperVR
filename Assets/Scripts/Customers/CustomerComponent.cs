using Assets.Scripts.Customers;
using Assets.Scripts.Customers.Rules;
using Assets.Scripts.Items;
using System.Collections;
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
    private ItemType itemType;
    [SerializeField] private Canvas textCanvas;

    private void Start()
    {
        textCanvas.enabled = false;
        Debug.Log("Customer getting request for day " + ProfitBoard.day);
        request = requestManager.GetRequest(customer, ProfitBoard.day);
        Debug.Log("GOT REQUEST" + request.name);

        nameText.text = customer.customerName;
        budgetText.text = "$" + customer.budget.ToString();
        typeText.text = string.Join("\n", customer.customerTypes);

        itemQualityText.text = request.minimumQuality.ToString();
        itemTypeText.text = request.requiredType.ToString();

        //requestText.text = customer.request.requestString()[0]; //make new text mesh for each one?
        requestText.text = string.Join("\n", request.requestString());
        Debug.Log("Happiness START AT" + customer.happiness);
        patience = customer.patience;
        //patienceText.text = "Patience " + customer.patience.ToString();
        SetPatienceDisplay(customer.patience);
        happinessFloat = customer.happiness;
        Debug.Log("customer component happiness " + happinessFloat);
        happinessManager = new CustomerHappinessManager(customer.lowFineHappiness, customer.highFineHappiness);
        CustomerHappiness initialHappiness = happinessManager.GetHapinessFromPatience(customer.happiness);
        //happinessText.text = initialHappiness.ToString();
        SetHappinessDisplay(initialHappiness);

        StartCoroutine(MoveCustomerTo(
            gameObject.transform,
            Station.CounterPoint.position,
            5f, 
            false
        ));

        ProfitBoard.OnDayEnded += DayEnded;

    }

    private void OnDestroy()
    {
        ProfitBoard.OnDayEnded -= DayEnded;

    }


    private IEnumerator MoveCustomerTo(Transform customer, Vector3 targetPos, float duration, bool destroyOnArrival)
    {
        Vector3 startPos = customer.position;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            customer.position = Vector3.Lerp(startPos, targetPos, t);

            time += Time.deltaTime;
            yield return null;
        }


        customer.position = targetPos;

        if (destroyOnArrival)
        {
            Destroy(customer.gameObject);
        }
    }

    public void Initialize(Customer data)
    {
        customer = data;
    }

    public void StartPatienceTimer()
    {
        patienceCoroutine = StartCoroutine(ReducePatience());
    }

    private System.Collections.IEnumerator ReducePatience()
    {
        yield return new WaitForSeconds(1f);
        textCanvas.enabled = true;
        Debug.Log("reduce patience happiness " + happinessFloat);
        Debug.Log("reduce patience patience " + patience);
        Debug.Log("reduce patience customer patience " + customer.patience);
        while (patience > 0)
        {
            yield return new WaitForSeconds(1f);

            patience -= 1;

            //patienceText.text = "Patience " + patience.ToString();
            SetPatienceDisplay((int)patience);
            float timeLeft = Mathf.Clamp01((float)patience / customer.patience);
            Debug.Log("happiness time left " +  timeLeft);
            happiness = happinessManager.GetHapinessFromPatience(timeLeft);
            happinessFloat = timeLeft;
            Debug.Log("happiness from patience timer " + happinessFloat);
            //happinessText.text = happiness.ToString();
            SetHappinessDisplay(happiness);

        }

        Debug.Log("Customer ran out of patience!");
    }

    public CustomerHappiness ReduceHappiness(float reduction)
    {
        Debug.Log("REduce happiness by " +  reduction);
        Debug.Log("ending happiness " + happinessFloat);
        happinessFloat += reduction;
        happiness = happinessManager.GetHapinessFromPatience(happinessFloat);
        Debug.Log("happiness value" + happinessFloat);
        Debug.Log("official happiness" + happiness.ToString());
        //happinessText.text = happiness.ToString();
        SetHappinessDisplay(happiness);
        return happiness;
    }

    private void StopPatienceTimer()
    {
        if (patienceCoroutine != null)
        {
            StopCoroutine(patienceCoroutine);
            patienceCoroutine = null;

            //StartCoroutine(MoveCustomerTo(
            //    gameObject.transform,
            //    Station.SpawnPoint.position,
            //    5f
            //));
        }
    }

    public void RequestFulfilled()
    {
        StopPatienceTimer();
        textCanvas.enabled = false;
        StartCoroutine(MoveCustomerTo(
               gameObject.transform,
               Station.SpawnPoint.position,
               5f,
               true
           ));
    }

    private void DayEnded()
    {
        RequestFulfilled();
    }

    private void SetPatienceDisplay(int patience)
    {
        patienceText.text = "Patience: " + patience.ToString();
    }

    private void SetHappinessDisplay(CustomerHappiness happiness)
    {
        happinessText.text = happiness.ToString();
    }
}