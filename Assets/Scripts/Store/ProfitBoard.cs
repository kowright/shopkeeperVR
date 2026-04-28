using Assets.Scripts.Customers;
using Assets.Scripts.Items;
using Assets.Scripts.Store;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;

public class ProfitBoard : MonoBehaviour
{

    public TextMeshProUGUI dayProfitText;
    public TextMeshProUGUI profitText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI dayText;
    public static int day => DayManager.day;


    /// <summary>
    /// Get time in seconds of how long the day countdown is
    /// </summary>
    private int dayTime => dayManager.daytime;  // 2 mins to start, every exyra day gives an extra minute
    //private int dayTime => 30; // testing

    [SerializeField] private List<SubmitTable> stations;
    private float storeProfit = 0;
    private float todayProfit = 0;
    private bool isStoreOpen = false;
    public static Action OnBusinessDayStarted;
    public static Action OnNextDay;
    public static Action OnDayEnded;
    private Coroutine openForBusinessCoroutine;
    private DayManager dayManager;
    private Station currentStation;

    void OnEnable()
    {
        countdownText.text = "Store closed";
        SubmitTable.OnTableSubmitted += AddDuringBusinessHoursProfit;
        ShelfTrigger.OnSpawnerPlaced += AddOutsideBusinessHoursProfit;
        Station.OnStationEnabled += StationChange;
        dayText.text = "Day: " + day.ToString();


    }

    void OnDisable()
    {
        SubmitTable.OnTableSubmitted -= AddDuringBusinessHoursProfit;
        ShelfTrigger.OnSpawnerPlaced -= AddOutsideBusinessHoursProfit;
        Station.OnStationEnabled -= StationChange;


    }

    private void Start()
    {

        dayManager = new();
        isStoreOpen = false;
        Debug.Log("rent: " + dayManager.rent);
        storeProfit -= dayManager.rent;
        dayProfitText.text = $"Today's Profit: ${storeProfit.ToString()}";

    }

    private void AddDuringBusinessHoursProfit(float amount)
    {
        if (!isStoreOpen)
        {
            Debug.Log("Can only get profit during the day");
            return;
        }

        Debug.Log("ADD PROFIT");
        todayProfit += amount;
        dayProfitText.text = $"Today's Profit: ${todayProfit}";
    }

    private void AddOutsideBusinessHoursProfit(int amount)
    {
        if (isStoreOpen)
        {
            Debug.Log("Can only add spawners outside work hours");
            return;
        }

        Debug.Log("ADD SPAWNER COST:" + amount);
        Debug.Log("previous store profit: " + storeProfit);
        storeProfit += amount;
        profitText.text = $"Store Profit: ${storeProfit}";
    }

    // is called on New Day Button- just allows stats to reset
    public void StartNewDay() // TODO make private again
    {
        if (isStoreOpen)
        {
            Debug.Log("Day is already started");
            return;
        }

        dayManager.SetNextDay();
        OnNextDay?.Invoke();
        dayText.text = "Day: " + day.ToString();
        dayProfitText.text = "";
        //Debug.Log("START THE DAY" + day);
        //isStoreOpen = true;
        //OnDayStarted?.Invoke();
        //StartOpenBusinessTimer();
    }

    public void OpenForBusiness()
    {
        Debug.Log("START THE DAY" + day);
        isStoreOpen = true;
        OnBusinessDayStarted?.Invoke();
        StartOpenBusinessTimer();

        //create customer timer 
        //Customer data = customerManager.CreateCustomerData();
        //station.SpawnCustomer(data);
    }

    public void StartOpenBusinessTimer()
    {
        openForBusinessCoroutine = StartCoroutine(OpenForBusinessCountdown());
    }

    private System.Collections.IEnumerator OpenForBusinessCountdown()
    {
        int time = dayTime;

        while (time > 0)
        {

            countdownText.text = "Store closes in: " + time.ToString();

            yield return new WaitForSeconds(1f);

            time -= 1;
        }

        //isStoreOpen = false;
        //countdownText.text = "Store closed!";
        //OnDayEnded?.Invoke();
        EndDay();
    }

    private void EndDay()
    {
        isStoreOpen = false;
        countdownText.text = "Store closed!";
        storeProfit += todayProfit;
        profitText.text = "Store Profit: $" + storeProfit.ToString();
        OnDayEnded?.Invoke();
    }

    private void StationChange(Station newStation)
    {
        Debug.Log("Station Change");
        currentStation = newStation;
    }
}
