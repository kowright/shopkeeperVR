using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;

public class ProfitBoard : MonoBehaviour
{

    public TextMeshProUGUI text;
    public static int day { get; private set; }

    private float dayTime;
    [SerializeField] private List<SubmitTable> stations;
    private float storeProfit;
    public static Action<int> OnDayStarted;

    void OnEnable()
    {
        Debug.Log("Profit board subscribe");
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
        text.text = $"Profit: ${storeProfit}";
    }

    public void StartNewDay() // TODO make private again
    {
        day++;
        Debug.Log("START THE DAY" + day);

        OnDayStarted?.Invoke(day);
    }
}
