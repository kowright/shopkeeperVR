using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Customers
{
    public class CustomerManager
    {
        private int businessDay => ProfitBoard.day;
        System.Random random = new System.Random();
        private ItemType itemType;

        private Dictionary<int, List<List<CustomerType>>> customerTypeGroupings = new Dictionary<int, List<List<CustomerType>>>()
        {
            // TODO: add specific for group 
             { 1, new List<List<CustomerType>>
                 {
                    new List<CustomerType> { CustomerType.Cheap },
                    new List<CustomerType> { CustomerType.Diverse },
                    new List<CustomerType> { CustomerType.Maximalist },
                    new List<CustomerType> { CustomerType.Minimalist },
                    new List<CustomerType> { CustomerType.Practical },
                    new List<CustomerType> { CustomerType.Diverse },
                 }
             },
             { 2, new List<List<CustomerType>>
                 {
 
                    new List<CustomerType> { CustomerType.Picky },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Cheap },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Cheap },
                    new List<CustomerType> { CustomerType.Diverse },
                    new List<CustomerType> { CustomerType.Average },
                    new List<CustomerType> { CustomerType.Specific },
                 }
             },
             // ---
             { 3, new List<List<CustomerType>>
                 {

                    new List<CustomerType> { CustomerType.Picky, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Cheap },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Cheap },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.Cheap },
                    new List<CustomerType> { CustomerType.Average, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Specific, CustomerType.Minimalist },
                 }
             },
             { 4, new List<List<CustomerType>>
                 {

                    new List<CustomerType> { CustomerType.Picky, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Cheap },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Average },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.Average },
                    new List<CustomerType> { CustomerType.Average, CustomerType.Bitter },
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.DebbyDowner },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Cheap, CustomerType.Bitter },
                 }
             },
             // ---
             { 5, new List<List<CustomerType>>
                 {
                    new List<CustomerType> { CustomerType.Picky, CustomerType.Impatient, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Cheap },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Average },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.Average, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Average, CustomerType.Impatient },
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.BigSpender },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Average, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.BigSpender, CustomerType.Demanding },

                 }
             },
             { 6, new List<List<CustomerType>>
                 {
                    new List<CustomerType> { CustomerType.Picky, CustomerType.Impatient, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Average },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.BigSpender, CustomerType.Bitter },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.Average },
                    new List<CustomerType> { CustomerType.Average, CustomerType.Impatient, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.BigSpender },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Average, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Average },

                 }
             },
             // --- GO CRAZY - +3 types!
             { 7, new List<List<CustomerType>>
                 {
                    new List<CustomerType> { CustomerType.Picky, CustomerType.Impatient, CustomerType.Minimalist, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Average, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.BigSpender },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.Average, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Average, CustomerType.Impatient, CustomerType.Diverse },
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.BigSpender, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Average, CustomerType.Picky },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Average },

                 }
             },
             { 8, new List<List<CustomerType>>
                 {
                    new List<CustomerType> { CustomerType.Picky, CustomerType.Impatient, CustomerType.Maximalist },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Cheap, CustomerType.Spontaneous, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.BigSpender, CustomerType.Picky },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.BigSpender, CustomerType.DebbyDowner },
                    new List<CustomerType> { CustomerType.Average, CustomerType.Impatient,CustomerType.Diverse, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.BigSpender, CustomerType.Picky },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Average, CustomerType.DebbyDowner, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Average, CustomerType.Diverse },

                 }
             },
             // getting unreasonable

             { 9, new List<List<CustomerType>>
                 {
                    new List<CustomerType> { CustomerType.Picky, CustomerType.Impatient, CustomerType.Maximalist, CustomerType.DebbyDowner },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Cheap, CustomerType.Picky, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Average, CustomerType.Spontaneous, CustomerType.Bitter },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Average, CustomerType.Picky, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.BigSpender, CustomerType.Patient, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Average, CustomerType.Impatient,CustomerType.Diverse },
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.BigSpender, CustomerType.Diverse, CustomerType.DebbyDowner },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Average, CustomerType.Bitter},
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Cheap, CustomerType.Diverse, CustomerType.Demanding },

                 }
             },

             { 10, new List<List<CustomerType>>
                 {
                    new List<CustomerType> { CustomerType.Picky, CustomerType.Impatient, CustomerType.Maximalist, CustomerType.Bitter, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Cheap, CustomerType.Picky, CustomerType.Bitter },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Cheap, CustomerType.Picky, CustomerType.Impatient, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Average, CustomerType.Spontaneous },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Cheap, CustomerType.Picky, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.BigSpender, CustomerType.Patient, CustomerType.DebbyDowner },
                    new List<CustomerType> { CustomerType.BigSpender, CustomerType.Impatient,CustomerType.Diverse },
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.BigSpender, CustomerType.Diverse, CustomerType.DebbyDowner },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Average,  CustomerType.Spontaneous, CustomerType.Picky, CustomerType.Demanding},
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Cheap, CustomerType.Diverse, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Cheap, CustomerType.Impatient, CustomerType.DebbyDowner },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Cheap, CustomerType.Minimalist, CustomerType.Diverse, CustomerType.DebbyDowner },


                 }
             },


        };



        public Customer CreateCustomerData()
        {
            List<CustomerType> customerTypes = GetCustomerTypes();
            int budget = GetCustomerBudget(customerTypes);
            int patience = GetCustomerPatience(customerTypes);
            (float happiness, float lowFine, float highFine) = GetCustomerFineHappiness(customerTypes);

            Customer newCustomer = new Customer(budget, patience, customerTypes, happiness, lowFine, highFine);
            return newCustomer;
        }

        private int GetCustomerBudget(List<CustomerType> customerTypes)
        {
            bool isCheap = customerTypes.Contains(CustomerType.Cheap); // lower 25% of range
            bool isAverage = customerTypes.Contains(CustomerType.Average); // middle 50% of range
            bool isBigSpender = customerTypes.Contains(CustomerType.BigSpender); // higher 25% of range

            (int low, int high) baseRange = businessDay switch
            {
                0 => (10, 20),
                1 => (20, 100),
                2 => (40, 200),
                3 => (60, 300),
                4 => (10, 500),
                5 => (10, 800),
                6 => (10, 1000),
                7 => (300, 2250),
                8 => (300, 4500),
                9 => (300, 6000),
                10 => (300, 10000),
                _ => (10, 1000)
            };

            (int adjustedLow, int adjustedHigh) = AdjustBudgetRange(baseRange.low, baseRange.high, customerTypes);

            return random.Next(adjustedLow, adjustedHigh);

            //switch (businessDay)
            //{
            //    case 0:
            //        int lowEnd;
            //        int highEnd;
            //        (lowEnd, highEnd) =  AdjustBudgetRange(10, 20, customerTypes);

            //        budget = random.Next(lowEnd, highEnd);
            //        return budget;
            //    case 1: return random.Next(20, 100);
            //    case 2: return random.Next(40, 200);
            //    case 3: return random.Next(60, 300);
            //    case 4: return random.Next(10, 500);
            //    case 5: return random.Next(10, 800);
            //    case 6: return random.Next(10, 1000);
            //    case 7: return random.Next(300, 2250);
            //    case 8: return random.Next(300, 4500);
            //    case 9: return random.Next(300, 5000);
            //    default:
            //        return 0;
        

        }
        private int GetCustomerPatience(List<CustomerType> customerTypes)
        {
            if (businessDay <= 2 && businessDay > 0)
            {
                return AdjustPatience(120, customerTypes);
            }
            if(businessDay >= 3 && businessDay < 5)
            {
                return AdjustPatience(100, customerTypes);

            }
            if (businessDay >=5 && businessDay < 7)
            {
                return AdjustPatience(60, customerTypes);

            }

            return AdjustPatience(30, customerTypes);
        }

        private int AdjustPatience(int initialPatience, List<CustomerType> customerTypes)
        {
            bool isImpatient = customerTypes.Contains(CustomerType.Impatient);
            bool isPatient = customerTypes.Contains(CustomerType.Patient);
            initialPatience = isImpatient ? (int)(initialPatience * 0.5f) : initialPatience;
            initialPatience = isPatient ? (int)(initialPatience * 1.5f) : initialPatience;
            return initialPatience;
        }

        private List<CustomerType> GetCustomerTypes()
        {

            if (customerTypeGroupings.TryGetValue(businessDay, out List<List<CustomerType>> possibleTypes))
            {
                int index = random.Next(possibleTypes.Count);
                return new List<CustomerType>(possibleTypes[index]);
            }
            return new List<CustomerType>() { CustomerType.Cheap };

        }

        private (int low, int high) AdjustBudgetRange(int lowEnd, int highEnd, List<CustomerType> customerTypes)
        {
            bool isCheap = customerTypes.Contains(CustomerType.Cheap);
            bool isAverage = customerTypes.Contains(CustomerType.Average);
            bool isBigSpender = customerTypes.Contains(CustomerType.BigSpender);

            int adjustedLow = lowEnd;
            int adjustedHigh = highEnd;

            if (isCheap)
                adjustedHigh = (int)(lowEnd * 1.25f);

            if (isBigSpender)
                adjustedLow = (int)(highEnd * 0.875f);

            if (isAverage)
            {
                adjustedHigh = (int)(highEnd * 0.875f);
                adjustedLow = (int)(lowEnd * 1.25f);
            }

            return (adjustedLow, adjustedHigh);
        }

        private (float happiness, float low, float high) GetCustomerFineHappiness(List<CustomerType> customerTypes)
        {
            bool isDebbyDowner = customerTypes.Contains(CustomerType.DebbyDowner);
            bool isBitter = customerTypes.Contains (CustomerType.Bitter);
            bool isForgiving = customerTypes.Contains(CustomerType.Forgiving);
            bool isDemanding = customerTypes.Contains(CustomerType.Demanding);
            bool isIndifferent = customerTypes.Contains(CustomerType.Indifferent);

            float lowFineHapiness = 0.25f;
            float highFineHappiness = 0.75f;
            float startingHappiness = 1.0f;

            if (isDemanding)
            {
                lowFineHapiness = 0.75f;
                highFineHappiness = 0.90f;
            }
            if (isForgiving)
            {
                lowFineHapiness = 0.10f;
                highFineHappiness = 0.25f;
            }
            if (isIndifferent)
            {
                lowFineHapiness = 0.10f;
                highFineHappiness = 0.90f;
            }


            if (isDebbyDowner)
            {
                startingHappiness = highFineHappiness;
            }

            if (isBitter)
            {
                startingHappiness = lowFineHapiness;
            }


            return (startingHappiness, lowFineHapiness, highFineHappiness);
        }
    }
}
