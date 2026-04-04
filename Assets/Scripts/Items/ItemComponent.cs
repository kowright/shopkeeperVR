using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [ExecuteAlways]
    public class ItemComponent : MonoBehaviour
    {
        public Item itemData;
        public TextMeshProUGUI text;
        private MeshRenderer outlineMeshRenderer;

        private ItemOutlineColorManager outlineColorManager = new ItemOutlineColorManager();

        private void Awake()
        {
            InitializeOutline();
            UpdateVisuals();

            //text.text = itemData.displayName;
            //Color outlineColor = outlineColorManager.GetOutlineColorForQuality(itemData.itemQuality);
            //Transform outlineTransform = transform.Find("Outline");
            //if (outlineTransform != null)
            //{
            //    outlineMeshRenderer = outlineTransform.GetComponent<MeshRenderer>();
            //    outlineMeshRenderer.material = new Material(outlineMeshRenderer.sharedMaterial);
            //    outlineMeshRenderer.material.SetColor("_OutlineColor", outlineColor);
            //    //outlineMeshRenderer.material.SetColor("_OutlineColor", outlineColor);

            //}
            //else
            //{
            //    Debug.LogWarning("Outline child not found!");
            //}
        }

        // editor only
        private void OnValidate()
        {
            InitializeOutline();
            UpdateVisuals();
            //text.text = itemData.displayName;
            //Color outlineColor = outlineColorManager.GetOutlineColorForQuality(itemData.itemQuality);

            //outlineMeshRenderer.material = new Material(outlineMeshRenderer.sharedMaterial);
            //outlineMeshRenderer.material.SetColor("_OutlineColor", outlineColor);
        }

        private void InitializeOutline()
        {
            if (outlineMeshRenderer == null)
            {
                Transform visualsTransform = transform.Find("Visuals");
                Transform outlineTransform = visualsTransform.Find("Outline");

                if (outlineTransform != null)
                    outlineMeshRenderer = outlineTransform.GetComponent<MeshRenderer>();
                else
                    Debug.LogWarning($"Outline child not found on {gameObject.name}!");
            }

            if (outlineColorManager == null)
            {
                outlineColorManager = new ItemOutlineColorManager();
            }
        }

        private void UpdateVisuals()
        {
            if (text != null && itemData != null)
            {
                text.text = itemData.displayName;
            }

            if (outlineMeshRenderer != null && itemData != null && outlineColorManager != null)
            {
                Color outlineColor = outlineColorManager.GetOutlineColorForQuality(itemData.itemQuality);

#if UNITY_EDITOR
                // In edit mode, create a new instance of the material to prevent shared material issues
                outlineMeshRenderer.sharedMaterial = new Material(outlineMeshRenderer.sharedMaterial);
                outlineMeshRenderer.sharedMaterial.SetColor("_OutlineColor", outlineColor);
#else
                outlineMeshRenderer.material = new Material(outlineMeshRenderer.sharedMaterial);
                outlineMeshRenderer.material.SetColor("_OutlineColor", outlineColor);
#endif
            }
        }
    }
}