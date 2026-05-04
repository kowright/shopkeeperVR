using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Customers
{
    public static class NamesRegistry
    {
        public static string GetRandomName()
        {
            System.Random rnd = new System.Random();
            int index = rnd.Next(Names.Count);
            return Names[index];
        }
         
        private static List<string> Names = new List<string>()
        {
            "John",
            "Mary",
            "Stephanie",
            "Brooklyn",
            "Steve",
            "Courtney",
            "Aaliyah",
            "Roger",
            "Tina",
            "Caleb",
            "Raven",
            "Sheila",
            "Danny",
            "Rodney",
            "Kelsey",
            "Peach",
            "Mario",
            "Eren",
            "Mikasa",
            "Armin",
            "Nathan",
            "Sora",
            "Brittany",
            "Ezio",
            "Connor",
            "Arno",
            "Evie",
            "Jacob",
            "Taila",
            "Cassandra",
            "Alexios",
            "Eivor",
            "Basim",
            "Altair",
            "Naoe",
            "Yasuke",

        };


    }
}
