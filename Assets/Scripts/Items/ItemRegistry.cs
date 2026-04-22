using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Item/ItemRegistry")]
    public class ItemRegistry : ScriptableObject
    {
        public List<Item> Items;
    }

}
