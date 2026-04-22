using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Store
{
    
    using global::Assets.Scripts.Items;
    using System.Collections;
    using TMPro;
    using UnityEngine;

    namespace Assets.Scripts.Store
    {
        public class ForPurchaseShelf : MonoBehaviour
        {
            [SerializeField] private ShelfTrigger TopShelfTrigger;
            [SerializeField] private ShelfTrigger MiddleShelfTrigger;
            [SerializeField] private ShelfTrigger BottomShelfTrigger;
            [SerializeField] private Transform BottomShelfLeft;
            [SerializeField] private Transform BottomShelfRight;
            [SerializeField] private Transform MiddleShelfLeft;
            [SerializeField] private Transform MiddleShelfRight;
            [SerializeField] private Transform TopShelfLeft;
            [SerializeField] private Transform TopShelfRight;
            [SerializeField] private ItemSpawner itemSpawnerPrefab;
            [SerializeField] private ItemRegistry itemRegistry;

            private int itemRegistryIndex = 0;

            private List<Transform> allPlacements;
            private Dictionary<Transform, Item> itemShelfPlacements;

            private void Awake()
            {
                allPlacements = new List<Transform>
                {
                    BottomShelfLeft,
                    BottomShelfRight,
                    MiddleShelfLeft,
                    MiddleShelfRight,
                    TopShelfLeft,
                    TopShelfRight
                };

                for (int i = 0; i < allPlacements.Count; i++ ) 
                {
                    ItemSpawner spawner = Instantiate(
                         itemSpawnerPrefab,
                         allPlacements[i].position,
                         allPlacements[i].rotation
                  
                     );

                    // Assign item AFTER spawning
                    spawner.Initialize(itemRegistry.Items[i]);
                    itemRegistryIndex++;
                }
            }

            // Use this for initialization
            void Start()
            {
                Debug.Log("registry index" + itemRegistryIndex);
         
                ProfitBoard.OnBusinessDayStarted += DayStarted;
                ProfitBoard.OnDayEnded += DayEnded;
            }

            private void OnDestroy()
            {
                ProfitBoard.OnBusinessDayStarted -= DayStarted;
                ProfitBoard.OnDayEnded -= DayEnded;

            }
            // Update is called once per frame
            void Update()
            {

            }

            private void DayStarted()
            {
                //if (BottomShelfTrigger != null)
                //{
                //    BottomShelfTrigger.SetSpawnersAsPurchasable();
                //}
                //if (TopShelfTrigger != null)
                //{
                //    TopShelfTrigger.SetSpawnersAsPurchasable();
                //}
                //if (MiddleShelfTrigger != null)
                //{
                //    MiddleShelfTrigger.SetSpawnersAsPurchasable();
                //}

                //TODO: put barrier on shelf to not allow spawns of items
           
            }

            private void DayEnded()
            {
                Debug.Log("day ended, renable shelf");
                this.gameObject.SetActive(true);
            }
        }
    }
}
