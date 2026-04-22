using Assets.Scripts.Items;
using Assets.Scripts.Store;
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
        public TextMeshProUGUI respawnTimerText;
        public Transform itemSpawnLocation;
        public MeshRenderer meshRenderer;
        private ItemOutlineColorManager outlineColorManager = new ItemOutlineColorManager();
        private ItemRespawnManager respawnManager = new ItemRespawnManager();
        public BoxCollider baseSpawnerCollider;
        [SerializeField] private bool isOutofPlace = false;
        [SerializeField] private XRGrabInteractable grabInteractable;

        public int SpawnerCost => item.cost * 10;

        public bool IsPaid { get; private set; } = false;

        //TODO: do not allow spawners to spawn items not during business day or allow it and delete all of them 

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
                Debug.LogWarning("ItemSpawner has no item assigned yet.");
                return;
            }

            itemPrefab = item.itemPrefab;
            nameText.text = item.displayName;
            typeTextLeft.text = typeTextRight.text = item.itemType.ToString();
            costText.text = '$' + item.cost.ToString();
            descriptionText.text = item.description;
            //itemPrefab = item?.itemPrefab;
            respawnTimerText.text = "";
            ItemComponent itemComponent = itemPrefab.GetComponent<ItemComponent>();
            Debug.Log("item c " +  itemComponent);
            Color materialColor = outlineColorManager.GetOutlineColorForQuality(item.itemQuality);

            Debug.Log("mesh renderer" + meshRenderer);

            Material mat = new Material(meshRenderer.sharedMaterials[0]);
            mat.color = materialColor;
            Color color = mat.color;
            color.a = 0.5f;
            mat.color = color;
            meshRenderer.material = mat;

            InstantiateItem();
        }

        //private void Awake()
        //{
        //    //item = itemPrefab.GetComponent<ItemComponent>();
        //    if (item == null)
        //    {
        //        Debug.LogWarning("ItemSpawner has no item assigned yet.");
        //        return;
        //    }

        //    itemPrefab = item.itemPrefab;
        //    nameText.text = item.displayName;
        //    typeTextLeft.text = typeTextRight.text = item.itemType.ToString();
        //    costText.text = '$' + item.cost.ToString();
        //    descriptionText.text = item.description;
        //    itemPrefab = item?.itemPrefab;
        //    respawnTimerText.text = "";
        //    Color materialColor = outlineColorManager.GetOutlineColorForQuality(item.itemQuality);



        //    Material mat = new Material(meshRenderer.sharedMaterials[0]);
        //    mat.color = materialColor;
        //    Color color = mat.color;
        //    color.a = 0.5f;
        //    mat.color = color;
        //    meshRenderer.material = mat;
        //    //Instantiate(item, itemSpawnLocation);
        //}

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ShelfTrigger>())
            {
                Debug.Log("Entered trigger");
                isOutofPlace = false;
                
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ShelfTrigger>())
            {
                Debug.Log("Left trigger");
                isOutofPlace = true;

            }
        }

        private void Start()
        {
            //InstantiateItem();
            ProfitBoard.OnBusinessDayStarted += DayStarted;
            ProfitBoard.OnDayEnded += ToggleGrabInteractivity;
        }

        private void OnDestroy()
        {
            ProfitBoard.OnBusinessDayStarted -= DayStarted;
            ProfitBoard.OnDayEnded -= ToggleGrabInteractivity;

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
            itemC.RefreshVisuals();


            Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        
        }

        private bool isRespawning = false;



        private void OnItemGrabbed(SelectEnterEventArgs args)
        {
            if (isRespawning) return;
          
            isRespawning = true;


            Transform itemTransform = args.interactableObject.transform;

            
            itemTransform.SetParent(null);

            Debug.Log("Item grabbed, starting respawn timer");

            ItemComponent item = args.interactableObject.transform.GetComponent<ItemComponent>();
            Debug.Log("Item " +  item);
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
            Debug.Log("respawn");
        }

        private System.Collections.IEnumerator RespawnTimerDisplay(float wait)
        {
            Debug.Log("timer");
            while (wait > 0)
            {
                yield return new WaitForSeconds(1f);

                wait -= 1;

                respawnTimerText.text = wait.ToString();
            }

            respawnTimerText.text = "";

        }

        public void SetSpawnerAsPaid()
        {
            Debug.Log("Spawner for " + item.displayName + " is paid");
            IsPaid = true;
         
        }

        private void DayStarted()
        {
            ToggleGrabInteractivity();
            if (isOutofPlace)
            {
                Debug.Log("not in the right place " + item.displayName);
                // took spawner off shelf and didn't pay/put it on usable shelf - TODO: so will this item never come back??
                Destroy(gameObject);
            }
        }

        private void ToggleGrabInteractivity()
        {
            Debug.Log(" grab for " + item.displayName + " as " + !baseSpawnerCollider.enabled);
            if (grabInteractable.interactionLayers == InteractionLayerMask.GetMask("None"))
            {
                grabInteractable.interactionLayers = InteractionLayerMask.GetMask("Default");
            }
            else
            {
                grabInteractable.interactionLayers = InteractionLayerMask.GetMask("None");
            }

        }
    }
}