using Assets.Scripts.Customers;
using Assets.Scripts.Items;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeField] private ItemType itemType;
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


    private void Awake()
    {
        SpawnPoint = customerSpawnPoint;
        CounterPoint = customerCounterTriggerPoint;

        switch (itemType)
        {
            case ItemType.Food: itemRegistry = foodRegistry; break;
            case ItemType.Weapon: itemRegistry = weaponRegistry; break;
            case ItemType.Accessory: itemRegistry = accessoryRegistry; break;
            default: break;
        }
        Debug.Log("station registry is " + itemRegistry);
    }

    private void OnEnable()
    {
        OnStationEnabled?.Invoke(this);
        customerManager = new(itemType);
        ProfitBoard.OnBusinessDayStarted += DayStarted;
        ProfitBoard.OnDayEnded += DayEnded;
    }

    private void OnDisable()
    {
        ProfitBoard.OnBusinessDayStarted += DayStarted;
        ProfitBoard.OnDayEnded += DayEnded;
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

    private void StopSpawningCustomers()
    {
        StopCoroutine(customerCreationCoroutine);
        customerCreationCoroutine = null;
    }

    private System.Collections.IEnumerator SpawnCustomers()
    {
        SpawnCustomer();
        while (openForBusiness) { 
            yield return new WaitForSeconds(15f);


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

 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
