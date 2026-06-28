using Assets.Scripts.Items;
using System;
using System.Collections.Generic;

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
                    new List<CustomerType> { CustomerType.Budgeter },
                    new List<CustomerType> { CustomerType.Picky, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Picky, CustomerType.Cheap },
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
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.Luxury },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Average, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.BigSpender, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.BigSpender, CustomerType.Minimalist },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Average },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Average },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.Average },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.Cheap },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Cheap },
                    new List<CustomerType> { CustomerType.Average, CustomerType.Maximalist, CustomerType.Practical },
                    new List<CustomerType> { CustomerType.Average, CustomerType.Practical },
                    new List<CustomerType> { CustomerType.Cheap, CustomerType.Patient },
                    new List<CustomerType> { CustomerType.Specific, CustomerType.Practical },
                    new List<CustomerType> { CustomerType.Specific, CustomerType.Average },
                    new List<CustomerType> { CustomerType.Specific, CustomerType.Patient },
                    new List<CustomerType> { CustomerType.Specific, CustomerType.Impatient },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Impatient },
                    new List<CustomerType> { CustomerType.Luxury, CustomerType.Patient, CustomerType.Minimalist },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Luxury },
                    new List<CustomerType> { CustomerType.Cheap, CustomerType.Minimalist },
                    new List<CustomerType> { CustomerType.Budgeter, CustomerType.Maximalist },

                 }
             },
             { 6, new List<List<CustomerType>>
                 {
                    new List<CustomerType> { CustomerType.Picky, CustomerType.Impatient, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Average },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.BigSpender, CustomerType.Bitter },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.Average },
                    new List<CustomerType> { CustomerType.Average, CustomerType.Impatient, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.Luxury },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Average, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Average },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Impatient},
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Impatient, CustomerType.Average},
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Impatient, CustomerType.Luxury},
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Impatient, CustomerType.BigSpender},
                    new List<CustomerType> { CustomerType.Specific, CustomerType.Impatient, CustomerType.Average},
                    new List<CustomerType> { CustomerType.Specific, CustomerType.Impatient, CustomerType.BigSpender},
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Impatient },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.Specific, CustomerType.Average},
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.Specific, CustomerType.Luxury},
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.Specific, CustomerType.Impatient},
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Luxury},
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.BigSpender},
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Impatient},
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.Average},
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Impatient, CustomerType.Minimalist },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Average, CustomerType.Spontaneous},
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Luxury, CustomerType.Spontaneous},
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Average, CustomerType.Spontaneous},
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.BigSpender, CustomerType.Impatient},
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Cheap, CustomerType.Impatient},
                    new List<CustomerType> { CustomerType.Budgeter, CustomerType.Impatient},
                    new List<CustomerType> { CustomerType.Budgeter, CustomerType.Diverse, },
                 }
             },
             // --- GO CRAZY - +3 types!
             { 7, new List<List<CustomerType>> // don't forget to be bitter or debbydowner
                 {
                    new List<CustomerType> { CustomerType.Picky, CustomerType.Impatient, CustomerType.Minimalist, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Average, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.BigSpender, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.Average, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Average, CustomerType.Impatient, CustomerType.Diverse },
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.BigSpender, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Average, CustomerType.Picky },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Average, CustomerType.DebbyDowner },
                    new List<CustomerType> { CustomerType.Practical, CustomerType.Average, CustomerType.Budgeter },
                    new List<CustomerType> { CustomerType.Demanding, CustomerType.BigSpender, CustomerType.Impatient },
                    new List<CustomerType> { CustomerType.Luxury, CustomerType.Patient, CustomerType.Minimalist },
                    new List<CustomerType> { CustomerType.Bitter, CustomerType.Luxury, CustomerType.Spontaneous },
                    new List<CustomerType> { CustomerType.Forgiving, CustomerType.BigSpender, CustomerType.Specific },
                    new List<CustomerType> { CustomerType.Indifferent, CustomerType.Cheap, CustomerType.Minimalist },
                    new List<CustomerType> { CustomerType.Average, CustomerType.Patient, CustomerType.Picky },
                    new List<CustomerType> { CustomerType.Demanding, CustomerType.Cheap, CustomerType.Maximalist },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Average, CustomerType.DebbyDowner },
                    new List<CustomerType> { CustomerType.Luxury, CustomerType.Forgiving, CustomerType.Impatient },
                    new List<CustomerType> { CustomerType.Bitter, CustomerType.BigSpender, CustomerType.Patient },
                    new List<CustomerType> { CustomerType.DebbyDowner, CustomerType.Average, CustomerType.Minimalist },
                    new List<CustomerType> { CustomerType.Budgeter, CustomerType.Spontaneous, CustomerType.Practical },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.BigSpender, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Average, CustomerType.Luxury },
                    new List<CustomerType> { CustomerType.Practical, CustomerType.Maximalist, CustomerType.Picky },
                    new List<CustomerType> { CustomerType.Budgeter, CustomerType.BigSpender, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Budgeter, CustomerType.Average, CustomerType.Indifferent },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Specific, CustomerType.Diverse },
                    new List<CustomerType> { CustomerType.Forgiving, CustomerType.BigSpender, CustomerType.Diverse },
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.Average, CustomerType.Diverse },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Cheap, CustomerType.Diverse },
                    new List<CustomerType> { CustomerType.Cheap, CustomerType.Luxury, CustomerType.Diverse },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Specific, CustomerType.Bitter },
                    new List<CustomerType> { CustomerType.Specific, CustomerType.Practical, CustomerType.Luxury },
                    new List<CustomerType> { CustomerType.Specific, CustomerType.Maximalist, CustomerType.Bitter },
                    new List<CustomerType> { CustomerType.Specific, CustomerType.Average, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Practical, CustomerType.Patient, CustomerType.Budgeter },
                    new List<CustomerType> { CustomerType.Cheap, CustomerType.DebbyDowner, CustomerType.Maximalist },

                 }
             },
             { 8, new List<List<CustomerType>> // mood + cost + patience + (risky, speed, items, group, budget, quality)  
             // mood - bitter, debbydowner, demanding, forgiving, indifferent
                 {
                    new List<CustomerType> { CustomerType.Picky, CustomerType.Impatient, CustomerType.Maximalist, CustomerType.Luxury },
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
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Luxury, CustomerType.Bitter},
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Cheap, CustomerType.Diverse, CustomerType.Demanding },

                 }
             },

             { 10, new List<List<CustomerType>>
                 {
                    new List<CustomerType> { CustomerType.Picky, CustomerType.Impatient, CustomerType.Maximalist, CustomerType.Bitter, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Cheap, CustomerType.Picky, CustomerType.Bitter },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Luxury, CustomerType.Picky, CustomerType.Impatient, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Maximalist, CustomerType.Average, CustomerType.Spontaneous },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Luxury, CustomerType.Picky, CustomerType.Demanding },
                    new List<CustomerType> { CustomerType.Diverse, CustomerType.BigSpender, CustomerType.Patient, CustomerType.DebbyDowner },
                    new List<CustomerType> { CustomerType.BigSpender, CustomerType.Impatient,CustomerType.Diverse },
                    new List<CustomerType> { CustomerType.Impatient, CustomerType.BigSpender, CustomerType.Diverse, CustomerType.DebbyDowner },
                    new List<CustomerType> { CustomerType.Patient, CustomerType.Luxury, CustomerType.Spontaneous, CustomerType.Picky, CustomerType.Demanding},
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Luxury, CustomerType.Diverse, CustomerType.Forgiving },
                    new List<CustomerType> { CustomerType.Minimalist, CustomerType.Luxury, CustomerType.Impatient, CustomerType.DebbyDowner },
                    new List<CustomerType> { CustomerType.Spontaneous, CustomerType.Luxury, CustomerType.Minimalist, CustomerType.Diverse, CustomerType.DebbyDowner },


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
                1 => (5, 15),
                2 => (6, 15),
                3 => (20, 40),
                4 => (20, 300),
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
