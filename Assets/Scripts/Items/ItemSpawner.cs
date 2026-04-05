using Assets.Scripts.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

namespace Assets.Scripts.Items
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject itemPrefab;
        private ItemComponent item => itemPrefab.GetComponent<ItemComponent>();
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
            nameText.text = item.itemData.displayName;
            typeTextLeft.text = typeTextLeft.text = item.itemData.itemType.ToString();
            costText.text = '$' + item.itemData.cost.ToString();
            descriptionText.text = item.itemData.description;
            //Instantiate(item, itemSpawnLocation);

            Color materialColor = outlineColorManager.GetOutlineColorForQuality(item.itemData.itemQuality);


            Debug.Log("renderer", meshRenderer);
      
            Material mat = new Material(meshRenderer.sharedMaterials[0]);
            Debug.Log("mat", mat);
            mat.color = materialColor;
            Color color = mat.color;
            color.a = 0.5f;
            mat.color = color;

            //mat.SetColor("_BaseColor", materialColor); // for URP


#if UNITY_EDITOR
            meshRenderer.sharedMaterial = mat;
#else
    meshRenderer.material = mat;
#endif
        }

        private void Awake()
        {
            nameText.text = item.itemData.displayName;
            typeTextLeft.text = typeTextRight.text = item.itemData.itemType.ToString();
            costText.text = '$' + item.itemData.cost.ToString();
            descriptionText.text = item.itemData.description;
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

            Debug.Log("Spawned Item" + spawnedItem.transform.localPosition.y);
            Debug.Log("Spawned Item" + spawnedItem.transform.position.y);


            Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();
            if (rb != null)
                rb.useGravity = false;
        }
    }
}