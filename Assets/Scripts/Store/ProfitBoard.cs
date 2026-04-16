using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;

public class ProfitBoard : MonoBehaviour
{

    public TextMeshProUGUI text;
    private float dayTime;
    [SerializeField] private List<SubmitTable> stations;
    private float storeProfit;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddStation(SubmitTable newTable)
    {
        Debug.Log("NEW TABLE");
        stations.Add(newTable);
    }

    public void RemoveStation(SubmitTable newTable)
    {
        stations.Remove(newTable);
    }
}
