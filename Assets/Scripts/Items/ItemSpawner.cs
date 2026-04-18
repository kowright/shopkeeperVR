using Assets.Scripts.Items;
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

        private void Awake()
        {
            //item = itemPrefab.GetComponent<ItemComponent>();
            itemPrefab = item.itemPrefab;
            nameText.text = item.displayName;
            typeTextLeft.text = typeTextRight.text = item.itemType.ToString();
            costText.text = '$' + item.cost.ToString();
            descriptionText.text = item.description;
            itemPrefab = item?.itemPrefab;
            respawnTimerText.text = "";
            Color materialColor = outlineColorManager.GetOutlineColorForQuality(item.itemQuality);


            Debug.Log("renderer", meshRenderer);

            Material mat = new Material(meshRenderer.sharedMaterials[0]);
            Debug.Log("mat", mat);
            mat.color = materialColor;
            Color color = mat.color;
            color.a = 0.5f;
            mat.color = color;
            meshRenderer.material = mat;
            //Instantiate(item, itemSpawnLocation);
        }

        private void Start()
        {
            InstantiateItem();
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

            XRGrabInteractable grab = spawnedItem.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                grab.selectEntered.AddListener(OnItemGrabbed);
            }

            ItemComponent itemC = spawnedItem.GetComponent<ItemComponent>();
            if (itemC == null)
            {
                Debug.Log("NO ITEM COMPONENT");
            }
            itemC.itemData = item;
            itemC.RefreshVisuals();

            Debug.Log("Spawned Item" + spawnedItem.transform.localPosition.y);
            Debug.Log("Spawned Item" + spawnedItem.transform.position.y);


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

                respawnTimerText.text = wait.ToString();
            }

            respawnTimerText.text = "";

        }
    }
}