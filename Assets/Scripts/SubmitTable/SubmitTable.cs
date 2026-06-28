using Assets.Scripts.Customers;
using Assets.Scripts.Items;
using Assets.Scripts.SubmitTable;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.PackageManager.Requests;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class SubmitTable: MonoBehaviour
{
    [SerializeField] private List<ItemComponent> itemsOnTable = new List<ItemComponent>();
    public TextMeshProUGUI results;
    public CustomerTriggerZone customerZone;
    private float tableRevenue; // how much table has made today
    public static Action<float> OnTableSubmitted;
    private Coroutine clearResultsCountdown;
    private int customersServed = 0;
    private int customersMadeHappy = 0;
    private int currentTableCost = 0;
    public TextMeshProUGUI priceCountText;
    public TextMeshProUGUI customerRequestText;
    public TextMeshProUGUI itemRequestText;
    private bool isOpenForBusiness;
    [SerializeField] private ItemComponent test;
    void Start()
    {
        ProfitBoard.OnDayEnded += SetBusinessClosedTable;
        ProfitBoard.OnNextDay += OnNextDay;
        CustomerTriggerZone.OnCustomerTriggerEnter += NewCustomer;
        ProfitBoard.OnBusinessDayStarted += OnDayStarted;
    }

    private void OnDestroy()
    {
        ProfitBoard.OnDayEnded -= SetBusinessClosedTable;
        ProfitBoard.OnNextDay -= OnNextDay;
        CustomerTriggerZone.OnCustomerTriggerEnter -= NewCustomer;
        ProfitBoard.OnBusinessDayStarted += OnDayStarted;
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponentInParent<ItemComponent>();
        if (item != null && isOpenForBusiness)
        {
            itemsOnTable.Add(item);
            Debug.Log("Adding " + item.itemData.displayName);
            currentTableCost += item.itemData.cost;
        
            SetTableCostText(currentTableCost);
        }
    }

    private void NewCustomer(CustomerComponent customer)
    {
        customerRequestText.text = string.Join("\n", customer.request.requestString());
    
        string itemQualityText = customer.request.minimumQuality == ItemQuality.None ? "" : customer.request.minimumQuality.ToString();
        string itemTypeText = customer.request.requiredType == ItemType.None ? "" : customer.request.requiredType.ToString();

        itemRequestText.text = itemQualityText != "" ? "Min Item Quality: " + itemQualityText : "";
        itemRequestText.text += itemTypeText != "" ? "\n\nReq Item Type: " + itemTypeText : "";
    }

    private void SetTableCostText(float cost)
    {
        priceCountText.text = "Current table cost:\n$" + cost.ToString();
        if (customerZone.currentCustomer != null)
        {
            if (customerZone.currentCustomer.budget >= currentTableCost)
            {
                priceCountText.color = Color.green;
            }
            else
            {
                priceCountText.color = Color.red;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var item = other.GetComponentInParent<ItemComponent>();
        Debug.Log($"Leaving {other.name}");
        if (item != null)
        {
            itemsOnTable.Remove(item);
            Debug.Log("removing " + item.itemData.displayName);
            currentTableCost -= item.itemData.cost;
            SetTableCostText(currentTableCost);
          
         
        }
    }

    public List<ItemComponent> GetItems() => itemsOnTable;

    public void ManualUISubmitItemsForValidation()
    {
        itemsOnTable = new List<ItemComponent>
        {
            test
        };
        SubmitItemsForValidation();
    }

    public void SubmitItemsForValidation()
    {
        if (itemsOnTable == null || itemsOnTable.Count <= 0) return;

        if (!customerZone)
        {
            Debug.Log("SubmitItemsForValidation no customer zone");
            return;
        }
        if (!customerZone.currentCustomerComponent)
        {
            Debug.Log("SubmitItemsForValidation no customer zone customer component");
            return;
        }
        customerZone.currentCustomerComponent.RequestFulfilled();

        customerRequestText.text = "";
        itemRequestText.text = "";
        results.text = "";


        var ( result, happinessReduction, moneyPaid) = ValidateSubmission(itemsOnTable, customerZone.currentCustomer);
      
        foreach (var item in itemsOnTable)
        {
           Destroy(item.gameObject);
            //maybe a sound or effect when destroyed
        }
     
        itemsOnTable.Clear();

        CustomerHappiness finalHappiness = customerZone.currentCustomerComponent.ReduceHappiness(happinessReduction);

        float storeTip = 0;
        bool isBigSpender = customerZone.currentCustomer.customerTypes.Contains(CustomerType.BigSpender);
        if(finalHappiness == CustomerHappiness.Fine)
        {
            storeTip = moneyPaid * 0.1f;
            storeTip = isBigSpender ? storeTip * 2 : storeTip;
            result.Add("Regular tip!");
            // maybe tip sound
        }
        else if (finalHappiness == CustomerHappiness.Happy)
        {
            storeTip = moneyPaid * 0.25f;
            storeTip = isBigSpender ? storeTip * 4 : storeTip;
            result.Add("Big tip!");
            customersMadeHappy += 1;
            // maybe tip sound
        }

        if (result.Count > 0)
        {
            //results.text = result[0];
            result.Add("$" + moneyPaid + "+ $" + storeTip);
            Debug.Log("results have " + result.Count + " strings called " + string.Join("\n", result));

            results.text =  string.Join("\n", result);

            //TODO: show all result text and add tip text
        }
        else
        {
            results.text = "$" + moneyPaid + "+ $" + storeTip;

        }

        Debug.Log("Tip is " +  storeTip);
        tableRevenue += storeTip;
        customersServed += 1;
        priceCountText.color = Color.white;
        priceCountText.text = "";
        currentTableCost = 0;
        priceCountText.text = "";
        OnTableSubmitted?.Invoke(storeTip);
        StartClearResultsCountdown();



    }

    public (List<string> result, float happinessReduction, int moneyPaid)  ValidateSubmission(List<ItemComponent> items, Customer customer)
    {
        // can reward for how much money left the person has
        // ALLOW MISTAKES TO GO THROUGH

       

        // 0. Cost
        int totalCost = 0;
        foreach (var item in items)
        {
            totalCost += item.itemData.cost;
        }
        int paid = Mathf.Min(totalCost, customer.budget);
        int loss = Mathf.Max(0, totalCost - customer.budget);
        int profit = paid - loss;
        Debug.Log("Table made " + profit);

        int customerMoneyLeft = customer.budget - totalCost;

        Debug.Log("customer budget is now " + customerMoneyLeft);
        List<string> results = new();
        if (customerMoneyLeft < 0)
        {
            string result = $"Overbudget by ${customerMoneyLeft}"; // TODO format correctly to be like -$2
            results.Add(result);
            return (results, -1.0f, 0);
            
        }
        //tableRevenue += totalCost;
        tableRevenue += profit;
        currentTableCost = 0;
        //OnTableSubmitted.Invoke(totalCost)
        OnTableSubmitted?.Invoke(profit);
    
        float happinessReduction = 0f;

        var request = customerZone.currentCustomerComponent.request;
        // 1. Required specific items
        if (request.requiredItems != null)
        {
            foreach (var reqItem in request.requiredItems)
            {
                if (!items.Exists(i => i.itemData == reqItem))
                {
                    //string result = $"Missing {reqItem.displayName}";

                    //return (result, -0.5f);
                    results.Add($"Missing {reqItem.displayName}");
                    happinessReduction += -0.5f;
                }
            }
        }

        // 2. Required type 
        if (request.hasRequiredType)
        {
            if (items.Exists(i => i.itemData.itemType != request.requiredType))
            {
                //string result = $"All items must be {request.requiredType}";
                //return (result, -0.2f);
                results.Add($"All items must be {request.requiredType}");
                happinessReduction += -0.2f;
            }
        }

        // 3. Minimum quality
        if (request.hasRequiredQuality)
        {
            if (items.Exists(i => i.itemData.itemQuality < request.minimumQuality))
            {
                //string result = $"All items must be at least {request.minimumQuality}";
                //return (result, -0.2f);
                results.Add($"All items must be at least {request.minimumQuality}");
                happinessReduction += -0.2f;
            }
            float totalBonus = 0f;

            foreach (var item in items)
            {
                int diff = item.itemData.itemQuality - request.minimumQuality;

                if (diff > 0)
                {
                    totalBonus += diff * 0.05f;
                }
            }

            // cap the bonus so it doesn't get crazy
            totalBonus = Mathf.Clamp(totalBonus, 0f, 0.3f);

            if (totalBonus > 0)
            {
                Debug.Log("quality bonus!");
                results.Add(customer.customerName + " is impressed by the quality!");
                happinessReduction += totalBonus;
            }

        }

        // 4. Extra rules (modular system)
        if (request.extraRules != null)
        {
            foreach (var rule in request.extraRules)
            {
                if (!rule.IsSatisfied(items, customer))
                {
                    //string result = $"Failed rule: {rule.name}";
                    //return (result, -0.2f);
                    results.Add(rule.FailureString);
                    happinessReduction += rule.FailureDeduction;
                }
            }
        }

        return (results, happinessReduction, paid);
    }

    private void SetBusinessClosedTable()
    {
        Debug.Log("DAY IS DONE");
        isOpenForBusiness = false;
        itemsOnTable.Clear();
        results.text = $"Station made: ${tableRevenue} \nCustomers served: {customersServed} \nCustomers made happy: {customersMadeHappy}";
        currentTableCost = 0;
        priceCountText.text = "";
        customerRequestText.text = "";

}

public void StartClearResultsCountdown()
    {
        clearResultsCountdown = StartCoroutine(ClearResultsText());
    }

    private System.Collections.IEnumerator ClearResultsText()
    {
        Debug.Log("start countdown");
        yield return new WaitForSeconds(5f);

        results.text = "";
        Debug.Log("end countdown");
    }

    private void OnNextDay()
    {
        results.text = "";
        customersMadeHappy = 0;
        customersServed = 0;
        currentTableCost = 0;
    }

    private void OnDayStarted()
    {
        isOpenForBusiness = true;
    }
}

