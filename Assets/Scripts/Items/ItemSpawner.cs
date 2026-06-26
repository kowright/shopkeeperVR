using Assets.Scripts.Items;
using Assets.Scripts.Store;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Assets.Scripts.Items
{
    public class ItemSpawner : MonoBehaviour
    {

        private GameObject itemPrefab;
        //public ItemComponent item; // doing it this way since every item scriptable object will use the same model 
        public Item item;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI typeTextLeft;
        public TextMeshProUGUI typeTextRight;
        public TextMeshProUGUI costText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI frontTimerText;
        public TextMeshProUGUI topTimerText;
        public Transform itemSpawnLocation;
        public MeshRenderer meshRenderer;
        private ItemOutlineColorManager outlineColorManager = new ItemOutlineColorManager();
        private ItemRespawnManager respawnManager = new ItemRespawnManager();
        public BoxCollider baseSpawnerCollider;
        [SerializeField] private bool isOnValidShelf = false;
        [SerializeField] private XRGrabInteractable spawnerGrabInteractable;
        private ItemComponent currentItem;
        private bool openForBusiness;
        private Coroutine removalCoroutine;
        private float removalDelay = 5f;
        // public Action OnSpawnerConfirmedRemoval;
        //public Action<float> OnSpawnerRemoved;
        //public Action OnSpawnerStopRemoval;
        //public Action OnSpawnerNeedsReplacement;

        public int SpawnerCost => item.cost * 10;

        public bool IsPaid { get; private set; } = false;
        //public bool HasBeenPlacedByPlayer { get; private set; }
        private bool isRespawning = false;
        //public void MarkPlacedByPlayer()
        //{
        //    HasBeenPlacedByPlayer = true;
        //}

        //public void UnmarkPlacedByPlayer()
        //{
        //    HasBeenPlacedByPlayer = false;
        //}

        private void Awake()
        {
            spawnerGrabInteractable.selectExited.AddListener(OnReleasedSpawner);
        }

        private void OnReleasedSpawner(SelectExitEventArgs args)
        {
            //if (!openForBusiness)
            //    return;

            //if (IsPaid)
            //    return;

            if (isOnValidShelf)
                return;

            if (spawnerGrabInteractable.isSelected)
                return;

            Debug.Log("Item Spawner released somewhere not valid! Start RemovalCheck on not grabbing it anymore!");
            Debug.Log("Item Spawner is it already being removed (ONRELEASE)" + (removalCoroutine != null));
            removalCoroutine = StartCoroutine(RemovalCheck(true));
        }


        private void OnValidate()
        {
            if (item == null) return;

            // Only update text if the references exist
            if (nameText != null)
                nameText.text = item.displayName;

            if (typeTextLeft != null && typeTextRight != null)
                typeTextLeft.text = typeTextRight.text = item.itemType.ToString();

            if (costText != null)
                costText.text = "$" + item.cost.ToString();

            if (descriptionText != null)
                descriptionText.text = item.description;


        }

        public void Initialize(Item newItem)
        {
            item = newItem;
            //item = itemPrefab.GetComponent<ItemComponent>();
            if (item == null)
            {
                Debug.LogWarning("Item Spawner has no item assigned yet.");
                return;
            }

            itemPrefab = item.itemPrefab;
            nameText.text = item.displayName;
            typeTextLeft.text = typeTextRight.text = item.itemType.ToString();
            costText.text = '$' + item.cost.ToString();
            descriptionText.text = item.description;
            //itemPrefab = item?.itemPrefab;
            frontTimerText.text = "";
            topTimerText.text = "";
            ItemComponent itemComponent = itemPrefab.GetComponent<ItemComponent>();
            Color materialColor = outlineColorManager.GetOutlineColorForQuality(item.itemQuality);


            Material mat = new Material(meshRenderer.sharedMaterials[0]);
            mat.color = materialColor;
            Color color = mat.color;
            color.a = 0.5f;
            mat.color = color;
            meshRenderer.material = mat;

            InstantiateItem();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ShelfTrigger>())
            {
                var shelf = other.GetComponent<ShelfTrigger>();

                if (shelf)
                {
                    
                   
                    isOnValidShelf = true;

                    if (shelf.ForPurchase && removalCoroutine != null)
                    {
                        //StopCoroutine(removalCoroutine);
                        removalCoroutine = null;
                        //OnSpawnerStopRemoval?.Invoke();
                    }
                }
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ShelfTrigger>())
            {
                isOnValidShelf = false;
                Debug.Log("Item Spawner removed out of ShelfTrigger for " + item.name + " Start RemovalCheck?");
                Debug.Log("Item Spawner is " + item.name + "already being removed? (LEFT SHELF TRIGGER)" + (removalCoroutine != null));
               // removalCoroutine = StartCoroutine(RemovalCheck());

            }
        }

        private void Start()
        {
            //InstantiateItem();
            ProfitBoard.OnBusinessDayStarted += DayStarted;
            ProfitBoard.OnDayEnded += OnDayEnded;

        }

        private void OnDestroy()
        {
            ProfitBoard.OnBusinessDayStarted -= DayStarted;
            ProfitBoard.OnDayEnded -= OnDayEnded;


        }

        public void InstantiateItem()
        {
            //ItemComponent itemC = itemPrefab.GetComponent<ItemComponent>();
            //if (itemC == null)
            //{
            //    Debug.Log("NO ITEM COMPONENT");
            //}
            //itemC.itemData = item;


            GameObject spawnedItem = Instantiate(
                 itemPrefab,
                 itemSpawnLocation.position,
                 itemSpawnLocation.rotation,
                 itemSpawnLocation
             );
            spawnedItem.transform.localPosition = Vector3.zero;
            spawnedItem.transform.localRotation = Quaternion.identity;
            spawnedItem.transform.localScale = Vector3.one;

            MonoBehaviour[] scripts = spawnedItem.GetComponents<MonoBehaviour>();

            //foreach (var script in scripts)
            //{
            //    Debug.Log(script.GetType().Name);
            //}

            XRGrabInteractable itemGrab = spawnedItem.GetComponent<XRGrabInteractable>();

            if (itemGrab != null)
            {
                itemGrab.selectEntered.AddListener(OnItemGrabbed);
            }

            ItemComponent itemC = spawnedItem.GetComponent<ItemComponent>();
            if (itemC == null)
            {
                Debug.Log("NO ITEM COMPONENT");
            }
            itemC.itemData = item;
            currentItem = itemC;
            XRGrabInteractable grab = currentItem.GetComponent<XRGrabInteractable>();

            //grab.interactionLayers = InteractionLayerMask.GetMask("None");

            ToggleInteractionLayer(grab, IsPaid || openForBusiness, false);

            itemC.RefreshVisuals();


            Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        
        }



        private void OnItemGrabbed(SelectEnterEventArgs args)
        {
            if (isRespawning) return;
          
            isRespawning = true;


            Transform itemTransform = args.interactableObject.transform;

            
            itemTransform.SetParent(null);
            currentItem = null;
            Debug.Log("Item grabbed, starting respawn timer");

            ItemComponent item = args.interactableObject.transform.GetComponent<ItemComponent>();
            if (item)
            {
                
                float time = respawnManager.GetRespawnTimeForQuality(item.itemData.itemQuality);
                StartCoroutine(RespawnAfterDelay(time));
                StartCoroutine(RespawnTimerDisplay(time));
            }

          
        }

        private System.Collections.IEnumerator RespawnAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            InstantiateItem();

            isRespawning = false;
         
        }

        private System.Collections.IEnumerator RespawnTimerDisplay(float wait)
        {
            while (wait > 0)
            {
                yield return new WaitForSeconds(1f);

                wait -= 1;

                frontTimerText.text = wait.ToString();
            }

            frontTimerText.text = "";

        }

        public void SetSpawnerAsPaid()
        {
            IsPaid = true;
            Debug.Log("Spawner is paid for");
            //XRGrabInteractable grab = currentItem.GetComponent<XRGrabInteractable>();
            //grab.interactionLayers = InteractionLayerMask.GetMask("Default");
            if (!currentItem) return;
            ToggleInteractionLayer(currentItem.GetComponent<XRGrabInteractable>(), true, false);

        }

        private void DayStarted()
        {
            openForBusiness = true;
            //ToggleGrabInteractivity();
            if (!isOnValidShelf)
            {
                Debug.Log("Item Spawner not in the right place " + item.displayName);
                Destroy(gameObject);
            }
            //XRGrabInteractable grab = currentItem.GetComponent<XRGrabInteractable>();

            //if (!IsPaid && grab != null)
            //{
            //    Debug.Log(item.displayName + " is now uninteractable");

            //    //grab.interactionLayers = InteractionLayerMask.GetMask("None");
            //    ToggleInteractionLayer(grab, false, false);
            //}
            //else
            //{
            //    grab.interactionLayers = InteractionLayerMask.GetMask("Default");
            //    ToggleInteractionLayer(grab, true, false);
            //}
        }

        private void OnDayEnded()
        {
            openForBusiness = false;
            //ToggleInteractionLayer(spawnerGrabInteractable, false, true);

            
        }

        //public void EnableInteraction()
        //{
        //    Debug.Log("Item Spawner enable interaction");
        //    ToggleInteractionLayer(spawnerGrabInteractable, true, false);

        //    if (currentItem != null)
        //    {
        //        ToggleInteractionLayer(
        //            currentItem.GetComponent<XRGrabInteractable>(),
        //            true,
        //            false
        //        );
        //    }
        //}

        private void ToggleInteractionLayer(XRGrabInteractable interactable, bool toDefault = false, bool invert = false)
        {
           // Debug.Log(
           //    $"Changing {interactable.gameObject.name} " +
           //    $"toDefault={toDefault} invert={invert}"
           //);
            if (invert)
            {
                if (interactable.interactionLayers == InteractionLayerMask.GetMask("None"))
                {
                    interactable.interactionLayers = InteractionLayerMask.GetMask("Default");
                }
                else
                {
                    interactable.interactionLayers = InteractionLayerMask.GetMask("None");
                }
                return;
            }

            
            if (toDefault)
            {
                interactable.interactionLayers = InteractionLayerMask.GetMask("Default");
            }
            else
            {
                interactable.interactionLayers = InteractionLayerMask.GetMask("None");
            }
        }

        /// <summary>
        /// Determines if ItemSpawner should be destroyed.
        /// </summary>
        /// <param name="spawnerRemoval"></param>
        /// <returns></returns>
        private System.Collections.IEnumerator RemovalCheck(bool spawnerRemoval = false)
        {
            Debug.Log("Item Spawner RemovalCheck Start for " + item.name);

            if (spawnerRemoval)
            {
                Debug.Log("Item Spawner countdown for spawner out of place for " + item.name);
            }
            Debug.Log("Item Spawner spawner grab interactable is Selected for " + item.name + "" + spawnerGrabInteractable.isSelected);
            // yield return new WaitForSeconds(removalDelay);
            float wait = removalDelay;
            while (wait > 0 && !spawnerGrabInteractable.isSelected)
            {

                yield return new WaitForSeconds(1f);

                wait -= 1;
               
           
                    topTimerText.text = wait.ToString();
                
                //OnSpawnerRemoved?.Invoke(wait);
            }
            topTimerText.text = "";
            // If STILL out of place after delay → confirm removal
        
            if (!isOnValidShelf && !spawnerGrabInteractable.isSelected)
            {
                Debug.Log("Item Spawner confirmed removed after delay for " + item.name);
                Debug.Log("Item Spawner RemovalCheck, ensure it is replaced!");
                //OnSpawnerConfirmedRemoval?.Invoke();
                //OnSpawnerNeedsReplacement.Invoke();

                Destroy(gameObject);
            }

            removalCoroutine = null;
        }
    }
}