using Assets.Scripts.Customers;
using Assets.Scripts.Items;
using Assets.Scripts.SubmitTable;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Store
{
	public class ShelfTrigger: MonoBehaviour
	{
        [SerializeField] private List<ItemSpawner> shelfSpawners = new List<ItemSpawner>();
        private float unpaidShelfCost;
        public TextMeshProUGUI shelfCostText;
        public static Action<int> OnSpawnerPlaced;
        public static Action<ItemSpawner> OnPurchaseShelfExit;
        public static Action<ItemSpawner> OnPurchaseShelfEnter;


        [SerializeField] private bool forPurchase;
        public bool ForPurchase => forPurchase;


        public List<ItemSpawner> GetItemSpawners() => shelfSpawners;

        void Start()
        {
            ProfitBoard.OnBusinessDayStarted += DayStarted;
            ProfitBoard.OnDayEnded += DayEnded;
        }

        private void OnDestroy()
        {
            ProfitBoard.OnBusinessDayStarted -= DayStarted;
            ProfitBoard.OnDayEnded -= DayEnded;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("ShelfTrigger OnTriggerEnter entered: " + other);
            var spawner = other.GetComponentInParent<ItemSpawner>();
            if (spawner != null)
            {
                if (shelfSpawners.Contains(spawner))
                {
                    return;
                }
                shelfSpawners.Add(spawner);


                if (forPurchase)
                {
                    if (!spawner.HasBeenPlacedByPlayer) return;

                    Debug.Log("Spawner entered purchase shelf");
                    OnPurchaseShelfEnter?.Invoke(spawner);
                    return;
                }

                Debug.Log("Adding spawner for item " + spawner.item.displayName);
                if (!spawner.IsPaid)
                {
                    unpaidShelfCost += spawner.SpawnerCost;
                    OnSpawnerPlaced?.Invoke(-1 * spawner.SpawnerCost);
                    spawner.SetSpawnerAsPaid();
                }

                shelfCostText.text = "Shelf Unpaid Cost: $" + unpaidShelfCost.ToString();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("ShelfTrigger OnTriggerExit");
            var spawner = other.GetComponentInParent<ItemSpawner>();
            //Debug.Log("Spawner for " + spawner.item.displayName + " is listed? " + shelfSpawners.Contains(spawner));
            if (spawner != null && shelfSpawners.Contains(spawner))
            {
                if (!spawner.HasBeenPlacedByPlayer)
                {
                    Debug.Log("Spawner " + spawner.nameText + " is not being used by player");
                    return;
                }

                shelfSpawners.Remove(spawner);
                Debug.Log("Removing spawner for item " + spawner.item.displayName);

                if (forPurchase)
                {
                    Debug.Log("Spawner exited purchase shelf");
                    if (spawner.IsPaid) return;
                    OnPurchaseShelfExit?.Invoke(spawner);
                    return;
                }

                if (!forPurchase)
                {
                   // spawner.EnableInteraction();

                    if (!spawner.IsPaid)
                    {
                        unpaidShelfCost += spawner.SpawnerCost;
                        OnSpawnerPlaced?.Invoke(-1 * spawner.SpawnerCost);
                    }

                    shelfCostText.text =
                        "Shelf Unpaid Cost: $" + unpaidShelfCost.ToString();
                }

                //if (!spawner.IsPaid)
                //{
                //    unpaidShelfCost -= spawner.SpawnerCost;
                //    OnSpawnerPlaced?.Invoke(spawner.SpawnerCost);
                //    spawner.SpawnerPlaced(0);
                //}

                //shelfCostText.text = "Shelf Unpaid Cost: $" + unpaidShelfCost.ToString();

            }
        }

        private void DayStarted()
        {
            if (forPurchase) return;

            shelfCostText.enabled = false;
            shelfCostText.text = "";
        }

        private void DayEnded()
        {
            if (forPurchase) return;

            shelfCostText.enabled = true;
        }

        public void SetSpawnersToPaid()
        {
            foreach (ItemSpawner spawner in shelfSpawners)
            {

                spawner.SetSpawnerAsPaid();
            }
        }


    }
}