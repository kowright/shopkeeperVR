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
            [SerializeField] private Transform BottomShelfMiddle;
            [SerializeField] private Transform MiddleShelfLeft;
            [SerializeField] private Transform MiddleShelfRight;
            [SerializeField] private Transform MiddleShelfMiddle;
            [SerializeField] private Transform TopShelfLeft;
            [SerializeField] private Transform TopShelfRight;
            [SerializeField] private Transform TopShelfMiddle;
            [SerializeField] private TextMeshProUGUI BottomShelfLeftText;
            [SerializeField] private TextMeshProUGUI BottomShelfMiddleText;
            [SerializeField] private TextMeshProUGUI BottomShelfRightText;
            [SerializeField] private TextMeshProUGUI MiddleShelfLeftText;
            [SerializeField] private TextMeshProUGUI MiddleShelfMiddleText;
            [SerializeField] private TextMeshProUGUI MiddleShelfRightText;
            [SerializeField] private TextMeshProUGUI TopShelfLeftText;
            [SerializeField] private TextMeshProUGUI TopShelfMiddleText;
            [SerializeField] private TextMeshProUGUI TopShelfRightText;

            [SerializeField] private ItemSpawner itemSpawnerPrefab;
            [SerializeField] private Station station;

            private List<Transform> allPlacements;
            private List<TextMeshProUGUI> allPlacementTexts;
            private Dictionary<Transform, Item> itemShelfPlacements;
            [SerializeField] private List<ItemRegistry> allItemRegistries;
            private List<ShelfSpot> shelfSpots;
            private Dictionary<ItemSpawner, ShelfSpot> spawnerToSpot = new Dictionary<ItemSpawner, ShelfSpot>();
            private Coroutine removalCoroutine;
            private float waitTime = 5f;

            private void Awake()
            {
                allPlacements = new List<Transform>
                {
                    BottomShelfLeft,
                    BottomShelfMiddle,
                    BottomShelfRight,
                    MiddleShelfLeft,
                    MiddleShelfMiddle,
                    MiddleShelfRight,
                    TopShelfLeft,
                    TopShelfMiddle,
                    TopShelfRight
                };

                allPlacementTexts = new List<TextMeshProUGUI>
                {
                    BottomShelfLeftText,
                    BottomShelfMiddleText,
                    BottomShelfRightText,
                    MiddleShelfLeftText,
                    MiddleShelfMiddleText,
                    MiddleShelfRightText,
                    TopShelfLeftText,
                    TopShelfMiddleText,
                    TopShelfRightText
                };
            }

            void Start()
            {
                shelfSpots = new List<ShelfSpot>();
           
                for (int i = 0; i < allPlacements.Count; i++)
                {
                    var spot = new ShelfSpot
                    {
                        placement = allPlacements[i],
                        registry = allItemRegistries[i],
                        currentIndex = 0,
                        unlockDay = allItemRegistries[i].unlockDay,
                        respawnText = allPlacementTexts[i],
              
                    };

                    if (ProfitBoard.day >= spot.unlockDay)
                    {
                        SpawnNextInSpot(spot);
                    }
                    shelfSpots.Add(spot);
                }



                ProfitBoard.OnBusinessDayStarted += DayStarted;
                ProfitBoard.OnDayEnded += DayEnded;
                ProfitBoard.OnNextDay += NextDay;
                ShelfTrigger.OnPurchaseShelfExit += HandlePurchaseShelfExit;
                ShelfTrigger.OnPurchaseShelfEnter += HandlePurchaseShelfEnter;

            }

            private void OnDestroy()
            {
                ProfitBoard.OnBusinessDayStarted -= DayStarted;
                ProfitBoard.OnDayEnded -= DayEnded;
                ProfitBoard.OnNextDay -= NextDay;
                ShelfTrigger.OnPurchaseShelfExit -= HandlePurchaseShelfExit;
                ShelfTrigger.OnPurchaseShelfEnter -= HandlePurchaseShelfEnter;


            }

            private void SpawnNextInSpot(ShelfSpot spot)
            {
                Debug.Log("Spawn Next In Spot");

                if (spot.currentIndex >= spot.registry.Items.Count)
                {
                    spot.currentIndex = 0;
                }

                if (ProfitBoard.day < spot.unlockDay)
                {
                    Debug.Log("Cannot spawn next item in registry since it is still locked");
                    return;
                }



                ItemSpawner spawner = Instantiate(
                    itemSpawnerPrefab,
                    spot.placement.position,
                    spot.placement.rotation
                );
                spawnerToSpot[spawner] = spot;

                Debug.Log("ForPurchaseShelf SpawnInNextSpot " + spot.registry.Items[spot.currentIndex].itemQuality + "" + spot.registry.Items[spot.currentIndex].itemGroup);
          
                spawner.Initialize(spot.registry.Items[spot.currentIndex]);

                //spawner.OnSpawnerConfirmedRemoval += () => HandleSpawnerConfirmedRemoved(spot);
                //spawner.OnSpawnerRemoved += (time) => HandleSpawnerInitialRemoval(spot, time);
                //spawner.OnSpawnerStopRemoval += () => HandleSpawnerStopRemoval(spot);

                spot.spawner = spawner;
            }

            private void HandleSpawnerConfirmedRemoved(ShelfSpot spot)
            {

                spot.spawner = null;
                spot.respawnText.gameObject.SetActive(false);
                
                if (ProfitBoard.day < spot.unlockDay)
                    return;
                Debug.Log("handle spawner confirmed removed 1");
                spot.currentIndex++;

                SpawnNextInSpot(spot);
            }

            private void HandleSpawnerInitialRemoval(ShelfSpot spot, float time)
            {
                spot.respawnText.gameObject.SetActive(true);
                Debug.Log("spot respawn text enabled " + spot.respawnText.enabled);
                spot.respawnText.text = time.ToString();
            }


            private void HandleSpawnerStopRemoval(ShelfSpot spot)
            {
                spot.respawnText.gameObject.SetActive(false);

                spot.respawnText.text = "";
            }

            private void DayStarted()
            {

           
            }

            private void NextDay()
            {
                UnlockSpawners();
            }

            private void UnlockSpawners()
            {

                foreach (ShelfSpot shelfSpot in shelfSpots)
                {
                    if (ProfitBoard.day >= shelfSpot.unlockDay && shelfSpot.spawner == null)
                    {
                        SpawnNextInSpot(shelfSpot);
                    }
                }
            }

            private void DayEnded()
            {
                this.gameObject.SetActive(true);
            }

            private void HandlePurchaseShelfExit(ItemSpawner spawner)
            {
                Debug.Log("ForPurchaseShelf HandlePurchaseShelfExit");

                Debug.Log("Spawner to Spots: ");
                foreach (var spawn in spawnerToSpot)
                {
                    Debug.Log("Spawner for item " + spawn.Key.item.displayName + " in spot " + spawn.Value);
                }
                if (!spawnerToSpot.TryGetValue(spawner, out var spot))
                {
                    Debug.Log("Could not find spot for spawner with items: " + spawner.item.displayName);
                    return;
                }

                removalCoroutine = StartCoroutine(RemovalCheck(spot));

                Debug.Log("ForPurchaseShelf HandlePurchaseExit Start Coroutine");
                // Mark spot empty
               // spot.spawner = null;

                // Remove mapping
               // spawnerToSpot.Remove(spawner);

                // Respect unlock rules
                //if (ProfitBoard.day < spot.unlockDay)
                //{
                //    Debug.Log("Cannot spawn next item in registry since it is still locked");
                //    return;
                //}
                

                // Move to next item in registry
                //spot.currentIndex++;

                // Spawn replacement
                //SpawnNextInSpot(spot);
            }

            private System.Collections.IEnumerator RemovalCheck(ShelfSpot spot)
            {
                // yield return new WaitForSeconds(removalDelay);
                float wait = waitTime;
                spot.respawnText.gameObject.SetActive(true);

                while (wait > 0)
                {
                    yield return new WaitForSeconds(1f);

                    wait -= 1;

                    spot.respawnText.text = wait.ToString();
                }

                Debug.Log("Removal Coroutine Completed Successfully");


                spot.respawnText.gameObject.SetActive(false);

       

                // Remove mapping
                Debug.Log("Spawner removed from spot: " + spot.placement.name);
                Debug.Log("spot spawner " + spot.spawner);
                spawnerToSpot.Remove(spot.spawner);

                spot.spawner = null;

                // Respect unlock rules
                //if (ProfitBoard.day < spot.unlockDay)
                //{
                //    Debug.Log("Cannot spawn next item in registry since it is still locked");
                //    return;
                //}


                // Move to next item in registry
                spot.currentIndex++;

                SpawnNextInSpot(spot);
            }

            private void HandlePurchaseShelfEnter(ItemSpawner spawner)
            {
                Debug.Log("ForPurchaseShelf HandlePurhcaseShelfEnter");
                if(removalCoroutine != null)
                {
                    StopCoroutine(removalCoroutine);
                    ShelfSpot spot = spawnerToSpot[spawner];
                    spot.respawnText.gameObject.SetActive(false);
                    spot.respawnText.text = waitTime.ToString();
                }
                else
                {
                    Debug.Log("restart routine");
                }
            }
        }
    }
}
