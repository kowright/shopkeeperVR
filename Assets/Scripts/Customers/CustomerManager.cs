using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Customers
{
    public class CustomerManager
    {
        private int businessDay => ProfitBoard.day;
        System.Random random = new System.Random();
        private ItemType itemType;

        public CustomerManager(ItemType itemType)
        {
            this.itemType = itemType;
        }

        public Customer CreateCustomerData()
        {
            
            int budget = GetCustomerBudget();
            Customer newCustomer = new Customer(budget, 120, new List<CustomerType> { CustomerType.Cheap }, 1.0f, .25f, .75f);
            return newCustomer;
        }

        private int GetCustomerBudget()
        {
            switch (businessDay)
            {
                case 0: return random.Next(10, 20);
                case 1: return random.Next(20, 100);
                case 2: return random.Next(40, 200);
                case 3: return random.Next(60, 300);
                case 4: return random.Next(10, 500);
                case 5: return random.Next(10, 800);
                case 6: return random.Next(10, 1000);
                case 7: return random.Next(10, 1250);
                case 8: return random.Next(10, 1500);
                case 9: return random.Next(10, 2000);
                default:
                    return 0;
            }

        }
        private void GetCustomerPatience()
        {

        }

        private void GetCustomerTypes()
        {

        }

        private (float low, float high) GetCustomerFineHappiness()
        {
            return (0f,0f);
        }
    }
}
