using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemComponent : MonoBehaviour
    {
        public Item itemData;
        public TextMeshProUGUI text;

        private void Start()
        {
            text.text = itemData.displayName;
        }
    }
}