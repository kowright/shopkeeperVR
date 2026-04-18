using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;

public class ProfitBoard : MonoBehaviour
{
   

    public TextMeshProUGUI profitText;
    public TextMeshProUGUI countdownText;
    public static int day { get; private set; }

    /// <summary>
    /// Get time in seconds of how long the day countdown is
    /// </summary>
    private int dayTime => (day * 60) + 120; // 2 mins to start, every exyra day gives an extra minute
    //private int dayTime => 30; // testing

    [SerializeField] private List<SubmitTable> stations;
    private float storeProfit;
    private bool isStoreOpen = false; // TODO should there be a specific number of customers each day or do as many customers as you can in the day? 
    //public static Action<int> OnDayStarted;
    public static Action OnDayEnded;
    private Coroutine openForBusinessCoroutine;

    void OnEnable()
    {
        countdownText.text = "Store closed";
        SubmitTable.OnTableSubmitted += AddProfit;
    }

    void OnDisable()
    {
        SubmitTable.OnTableSubmitted -= AddProfit;
    }

    private void AddProfit(float amount)
    {
        if (!isStoreOpen)
        {
            Debug.Log("Can only get profit during the day");
            return;
        }

        Debug.Log("ADD PROFIT");
        storeProfit += amount;
        profitText.text = $"Profit: ${storeProfit}";
    }

    // is called on Open For Business button
    public void StartNewDay() // TODO make private again
    {
        if (isStoreOpen)
        {
            Debug.Log("Day is already started");
            return;
        }
        day++;
        Debug.Log("START THE DAY" + day);
        isStoreOpen = true;
        //OnDayStarted?.Invoke(day);
        StartOpenBusinessTimer();
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

        isStoreOpen = false;
        countdownText.text = "Store closed!";
        OnDayEnded?.Invoke();
    }
}
