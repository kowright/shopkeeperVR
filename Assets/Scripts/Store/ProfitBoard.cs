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
    private float dayTime => (day * 60) + 120; // 2 mins to start, every exyra day gives an extra minute

    [SerializeField] private List<SubmitTable> stations;
    private float storeProfit;
    private bool isStoreOpen = false; // TODO should there be a specific number of customers each day or do as many customers as you can in the day? 
    public static Action<int> OnDayStarted;
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

    private void AddProfit(int amount)
    {
        Debug.Log("ADD PROFIT");
        storeProfit += amount;
        profitText.text = $"Profit: ${storeProfit}";
    }

    // is called on Open For Business button
    public void StartNewDay() // TODO make private again
    {
        day++;
        Debug.Log("START THE DAY" + day);
        isStoreOpen = true;
        OnDayStarted?.Invoke(day);
        StartOpenBusinessTimer();
    }

    public void StartOpenBusinessTimer()
    {
        openForBusinessCoroutine = StartCoroutine(OpenForBusinessCountdown());
    }

    private System.Collections.IEnumerator OpenForBusinessCountdown()
    {
        float time = dayTime;

        while (time > 0)
        {
            yield return new WaitForSeconds(1f);

            time -= 1;

            countdownText.text = "Store closes in: " + time.ToString();

        }

        isStoreOpen = false;
        countdownText.text = "Store closed!";
    }
}
