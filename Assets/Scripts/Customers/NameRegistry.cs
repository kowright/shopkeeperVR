using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Customers
{
    [CreateAssetMenu(fileName = "Name", menuName = "Customer/NameRegistry")]
    public class NameRegistry : ScriptableObject
    {
        public List<string> Names;

        public string GetRandomName()
        {
            System.Random rnd = new System.Random();
            int index = rnd.Next(Names.Count);
            return Names[index];
        }
    }
}
