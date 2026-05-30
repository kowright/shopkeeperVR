using Assets.Scripts.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Customers
{
    public class CustomerSpawnManager: MonoBehaviour
    {
        [SerializeField] private Transform[] queueSpots;
        private List<CustomerComponent> customerQueue = new List<CustomerComponent>();
        public static Transform SpawnPoint { get; private set; }

        [SerializeField] private Transform customerSpawnPoint;
        [SerializeField] private Transform customerCounterTriggerPoint;
        [SerializeField] private Transform customerCounterRightPoint;
        [SerializeField] private Transform customerCounterLeftPoint;
        private List<Transform> customerTurnAroundPoints;
        private CustomerManager customerManager;
        private Coroutine customerCreationCoroutine;
        private bool openForBusiness = false;
        [SerializeField] private GameObject customerPrefab;
        private float customerSpawnInterval => DayManager.daySpawnRate;
        System.Random random = new System.Random();

        private void Awake()
        {
            SpawnPoint = customerSpawnPoint;
            customerTurnAroundPoints = new List<Transform>
            {
                customerCounterRightPoint,
                customerCounterLeftPoint
            };

        }

        private void OnEnable()
        {

            customerManager = new CustomerManager();

          
            ProfitBoard.OnBusinessDayStarted += DayStarted;
            ProfitBoard.OnDayEnded += DayEnded;


        }

        private void OnDisable()
        {
            ProfitBoard.OnBusinessDayStarted -= DayStarted;
            ProfitBoard.OnDayEnded -= DayEnded;
 
        }

        private void DayStarted()
        {
            openForBusiness = true;
            customerCreationCoroutine = StartCoroutine(SpawnCustomers());
        }

        private void DayEnded()
        {
            openForBusiness = false;
            StopSpawningCustomers();
            SendBackCustomers();
            customerQueue.Clear();
        }

        private void StopSpawningCustomers()
        {
            StopCoroutine(customerCreationCoroutine);
            customerCreationCoroutine = null;
        }

        private void SendBackCustomers()
        {
            for (int i = 0; i < customerQueue.Count; i++)
            {
                customerQueue[i].MoveTo(customerSpawnPoint.position);
                Destroy(customerQueue[i].gameObject, 5f);
            }
            
        }

        private System.Collections.IEnumerator SpawnCustomers()
        {
            SpawnCustomer();
            while (openForBusiness)
            {
                yield return new WaitForSeconds(customerSpawnInterval); 


                SpawnCustomer();

            }
        }

        public void SpawnCustomer()
        {
            if (customerQueue.Count >= queueSpots.Length)
            {
                Debug.Log("Queue full");
                return;
            }

            Customer customerData = customerManager.CreateCustomerData();

            GameObject prefab = Instantiate(
                customerPrefab,
                customerSpawnPoint.position,
                Quaternion.identity
            );

            CustomerComponent customer = prefab.GetComponent<CustomerComponent>();

            if (customer != null)
            {
                customer.Initialize(customerData);
                customer.OnRequestFulfilled += CustomerRequestFulfilled; // TODO how to unsubscribe?

                customerQueue.Add(customer);

                UpdateQueuePositions();
            }
        }

        private void UpdateQueuePositions()
        {
            for (int i = 0; i < customerQueue.Count; i++)
            {
                customerQueue[i].MoveTo(queueSpots[i].position);
            }
        }

        private void CustomerRequestFulfilled()
        {
            StartCoroutine(HandleCustomerExit());
        }

        private IEnumerator HandleCustomerExit()
        {
            if (customerQueue[0] == null) yield break;

            CustomerComponent frontCustomer = customerQueue[0];

            customerQueue.RemoveAt(0);

            int index = random.Next(customerTurnAroundPoints.Count);
            Transform turnAroundPoint = customerTurnAroundPoints[index];
            frontCustomer.MoveTo(turnAroundPoint.position);

            // Wait until they are clear of the counter
            yield return new WaitForSeconds(1f);


            frontCustomer.MoveTo(turnAroundPoint.position + (Vector3.right * 10f));

            UpdateQueuePositions();

            Destroy(frontCustomer.gameObject, 5f);
        }
    }
}
