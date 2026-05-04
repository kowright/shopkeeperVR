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

            private int itemRegistryIndex = 0;

            private List<Transform> allPlacements;
            private List<TextMeshProUGUI> allPlacementTexts;
            private Dictionary<Transform, Item> itemShelfPlacements;
            [SerializeField] private List<ItemRegistry> allItemRegistries;
            private List<ShelfSpot> shelfSpots;

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

            private void SpawnNextInSpot(ShelfSpot spot)
            {
                if (spot.currentIndex >= spot.registry.Items.Count)
                {
                    spot.currentIndex = 0;
                }

                ItemSpawner spawner = Instantiate(
                    itemSpawnerPrefab,
                    spot.placement.position,
                    spot.placement.rotation
                );

                spawner.Initialize(spot.registry.Items[spot.currentIndex]);

                spawner.OnSpawnerConfirmedRemoval += () => HandleSpawnerConfirmedRemoved(spot);
                spawner.OnSpawnerRemoved += (time) => HandleSpawnerInitialRemoval(spot, time);
                spawner.OnSpawnerStopRemoval += () => HandleSpawnerStopRemoval(spot);

                spot.spawner = spawner;
            }

            private void HandleSpawnerConfirmedRemoved(ShelfSpot spot)
            {

                spot.spawner = null;
                spot.respawnText.gameObject.SetActive(false);

                if (ProfitBoard.day < spot.unlockDay)
                    return;

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
            // Use this for initialization
            void Start()
            {
                shelfSpots = new List<ShelfSpot>();
                Debug.Log("PROFIT DAY " + ProfitBoard.day);
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
                    Debug.Log("Spot " + i + " unlock day " + spot.unlockDay);
                    if (ProfitBoard.day >= spot.unlockDay)
                    {
                        Debug.Log("can spawn " + spot.registry.name);
                        SpawnNextInSpot(spot);
                    }
                    shelfSpots.Add(spot);
                }
               


                ProfitBoard.OnBusinessDayStarted += DayStarted;
                ProfitBoard.OnDayEnded += DayEnded;
                ProfitBoard.OnNextDay += NextDay;
            }

            private void OnDestroy()
            {
                ProfitBoard.OnBusinessDayStarted -= DayStarted;
                ProfitBoard.OnDayEnded -= DayEnded;
                ProfitBoard.OnNextDay -= NextDay;

            }
            // Update is called once per frame
            void Update()
            {

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
                Debug.Log("day ended, renable shelf");
                this.gameObject.SetActive(true);
            }
        }
    }
}
