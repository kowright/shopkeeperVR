using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Store
{
    class ShelfSpot
    {
        public Transform placement;
        public ItemRegistry registry;
        public int currentIndex;
        public ItemSpawner spawner;
        public int unlockDay;
        public TextMeshProUGUI respawnText;
    }
}
