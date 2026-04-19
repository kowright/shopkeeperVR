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
        private bool hideShelfCostText = false;
        public static Action<int> OnSpawnerPlaced;


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
                Debug.Log("Adding spawner for item " + spawner.item.displayName);
                if (!spawner.IsPaid)
                {
                    unpaidShelfCost += spawner.SpawnerCost;
                    OnSpawnerPlaced?.Invoke(-spawner.SpawnerCost);
                }

                shelfCostText.text = "Shelf Unpaid Shelves: $" + unpaidShelfCost.ToString();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var spawner = other.GetComponentInParent<ItemSpawner>();
            if (spawner != null)
            {
                shelfSpawners.Remove(spawner);
                Debug.Log("Removing spawner for item " + spawner.item.displayName);
                if (!spawner.IsPaid)
                {
                    unpaidShelfCost -= spawner.SpawnerCost;
                }

                shelfCostText.text = unpaidShelfCost.ToString();
            }
        }

        private void DayStarted()
        {
            hideShelfCostText = true;
            shelfCostText.enabled = false;
            shelfCostText.text = "";
        }

        private void DayEnded()
        {
            hideShelfCostText = false;
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