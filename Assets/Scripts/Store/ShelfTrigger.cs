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

        [SerializeField] private bool forPurchase;

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
            Debug.Log("Got " + other);
            var spawner = other.GetComponentInParent<ItemSpawner>();
            if (spawner != null)
            {
                if (shelfSpawners.Contains(spawner))
                {
                    return;
                }
                shelfSpawners.Add(spawner);

                if (forPurchase) return;

                Debug.Log("Adding spawner for item " + spawner.item.displayName);
                if (!spawner.IsPaid)
                {
                    unpaidShelfCost += spawner.SpawnerCost;
                    OnSpawnerPlaced?.Invoke(-1 * spawner.SpawnerCost);
                }

                shelfCostText.text = "Shelf Unpaid Cost: $" + unpaidShelfCost.ToString();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var spawner = other.GetComponentInParent<ItemSpawner>();
            if (shelfSpawners.Contains(spawner))
            {
                shelfSpawners.Remove(spawner);
                Debug.Log("Removing spawner for item " + spawner.item.displayName);

                if (forPurchase) return;

                if (!spawner.IsPaid)
                {
                    unpaidShelfCost -= spawner.SpawnerCost;
                    OnSpawnerPlaced?.Invoke(spawner.SpawnerCost);

                }

                shelfCostText.text = "Shelf Unpaid Cost: $" + unpaidShelfCost.ToString();

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