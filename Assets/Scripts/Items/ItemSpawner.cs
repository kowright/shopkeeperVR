using Assets.Scripts.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

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
        public Transform itemSpawnLocation;
        public MeshRenderer meshRenderer;
        private ItemOutlineColorManager outlineColorManager = new ItemOutlineColorManager();

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
        //        private void OnValidate()
        //        {
        //            //item = itemPrefab.GetComponent<ItemComponent>();

        //            Debug.Log("item" +  item.itemPrefab);
        //            itemPrefab = item.itemPrefab;
        //            nameText.text = item.displayName;
        //            typeTextLeft.text = typeTextRight.text = item.itemType.ToString();
        //            costText.text = '$' + item.cost.ToString();
        //            descriptionText.text = item.description;
        //            //Instantiate(item, itemSpawnLocation);

        //            Color materialColor = outlineColorManager.GetOutlineColorForQuality(item.itemQuality);


        //            Debug.Log("renderer", meshRenderer);

        //            Material mat = new Material(meshRenderer.sharedMaterials[0]);
        //            Debug.Log("mat", mat);
        //            mat.color = materialColor;
        //            Color color = mat.color;
        //            color.a = 0.5f;
        //            mat.color = color;

        //            //mat.SetColor("_BaseColor", materialColor); // for URP


        //#if UNITY_EDITOR
        //            meshRenderer.sharedMaterial = mat;
        //#else
        //    meshRenderer.material = mat;
        //#endif
        //        }

        private void Awake()
        {
            //item = itemPrefab.GetComponent<ItemComponent>();
            itemPrefab = item.itemPrefab;
            nameText.text = item.displayName;
            typeTextLeft.text = typeTextRight.text = item.itemType.ToString();
            costText.text = '$' + item.cost.ToString();
            descriptionText.text = item.description;
            itemPrefab = item?.itemPrefab;
            //Instantiate(item, itemSpawnLocation);
        }

        private void Start()
        {
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            //cube.transform.position = itemSpawnLocation.position;
            //cube.transform.rotation = itemSpawnLocation.rotation;
            //GameObject item = itemPrefab;
            //Rigidbody rb = item.GetComponent<Rigidbody>();
            //rb.useGravity = false;

            //item.transform.SetParent(itemSpawnLocation);

            //Instantiate(item, itemSpawnLocation.position, itemSpawnLocation.rotation);
            //Instantiate(item);

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

            foreach (var script in scripts)
            {
                Debug.Log(script.GetType().Name);
            }

            ItemComponent itemC = spawnedItem.GetComponent<ItemComponent>();
            if( itemC == null)
            {
                Debug.Log("NO ITEM COMPONENT");
            }
            itemC.itemData = item;

            Debug.Log("Spawned Item" + spawnedItem.transform.localPosition.y);
            Debug.Log("Spawned Item" + spawnedItem.transform.position.y);


            Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
            if (rb != null)
                rb.useGravity = false;
        }
    }
}