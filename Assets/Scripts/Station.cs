using Assets.Scripts.Customers;
using Assets.Scripts.Items;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Interactions;

public class Station : MonoBehaviour
{

    [SerializeField] private ItemRegistry foodRegistry;
    [SerializeField] private ItemRegistry weaponRegistry;
    [SerializeField] private ItemRegistry accessoryRegistry;
    private ItemRegistry itemRegistry;
    [SerializeField] private List<CustomerComponent> customerComponents;
    [SerializeField] private GameObject customerPrefab;

    public ItemRegistry ItemRegistry => itemRegistry;
    [SerializeField] private Transform customerSpawnPoint;
    [SerializeField] private Transform customerCounterTriggerPoint;

    public static Transform SpawnPoint { get; private set; }
    public static Transform CounterPoint { get; private set; }

    public static Action<Station> OnStationEnabled;
    private CustomerManager customerManager;
    private bool openForBusiness = false;
    private Coroutine customerCreationCoroutine;

    [SerializeField] private GameObject anotherShelfTable;
    [SerializeField] private Transform anotherShelfTableSecondaryPosition;
    [SerializeField] private GameObject secondShelf;
    [SerializeField] private GameObject thirdShelf;
    private int secondShelfAvailableDay = 3;
    private int thirdShelfAvailableDay = 6;

    private int day => ProfitBoard.day;


    private void Awake()
    {
        SpawnPoint = customerSpawnPoint;
        CounterPoint = customerCounterTriggerPoint;


        
    }

    private void OnEnable()
    {
        OnStationEnabled?.Invoke(this);
        customerManager = new CustomerManager();
        ProfitBoard.OnBusinessDayStarted += DayStarted;
        ProfitBoard.OnDayEnded += DayEnded;
        ProfitBoard.OnNextDay += NextDay;
    }

    private void OnDisable()
    {
        ProfitBoard.OnBusinessDayStarted -= DayStarted;
        ProfitBoard.OnDayEnded -= DayEnded;
        ProfitBoard.OnNextDay -= NextDay;
    }

    private void DayStarted()
    {
        openForBusiness = true;
        customerCreationCoroutine = StartCoroutine(SpawnCustomers());

    }

    private void DayEnded()
    {
        StopSpawningCustomers();
        openForBusiness = false;
    }

    private void NextDay()
    {
        if (!secondShelf.activeSelf && day >= secondShelfAvailableDay)
        {
            anotherShelfTable.SetActive(true);
        }
        if (secondShelf.activeSelf && day >= thirdShelfAvailableDay)
        {
            anotherShelfTable.SetActive(true);
        }

    }

    private void StopSpawningCustomers()
    {
        StopCoroutine(customerCreationCoroutine);
        customerCreationCoroutine = null;
    }

    private System.Collections.IEnumerator SpawnCustomers()
    {
        SpawnCustomer();
        while (openForBusiness) {  
            yield return new WaitForSeconds(30f); // TODO: reduce spawn rate with higher levels


            SpawnCustomer();

        }
    }

    public void SpawnCustomer()
    {
        Customer customerData = customerManager.CreateCustomerData();

        GameObject prefab = Instantiate(
            customerPrefab,
            customerSpawnPoint.position,
            Quaternion.identity
        );

        CustomerComponent customerComponent = prefab.GetComponent<CustomerComponent>();
        if (customerComponent != null)
        {
    
            customerComponent.Initialize(customerData);
        }

        //StartCoroutine(MoveToCounter(
        //    prefab.transform,
        //    customerCounterTriggerPoint.position,
        //    5f
        //));
    }

    public void AddNewShelf()
    {
        if (!secondShelf.activeSelf)
        {
            secondShelf.SetActive(true);
            anotherShelfTable.transform.position = anotherShelfTableSecondaryPosition.position;
            anotherShelfTable.SetActive(false);
            return;
        }
        // is third shelf
        thirdShelf.SetActive(true);
        anotherShelfTable.SetActive(false);
    }

 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
