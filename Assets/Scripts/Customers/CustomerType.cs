using Assets.Scripts.Customers.Rules;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Customers
{
    /// <summary>
    /// Personality types for a customer.
    /// </summary>
    /// <summary>
    /// Personality types for a customer.
    /// </summary>
    public enum CustomerType
    {

        /// <summary>Spends under $20. Equals Request Tag Cheap.</summary>
        Cheap,

        /// <summary>Spends under $100. Equals RequestTag MidRange.</summary>
        Average,

        /// <summary>Spends under $500. Equals Request Tag HighRange.</summary>
        Luxury,

        /// <summary>High spending customer. Equals Request Tag Expensive.</summary>
        BigSpender,

        /// <summary>Prefers high-quality items. Equals RequestTag HighQuality.</summary>
        Picky,

        /// <summary>Prefers mid-quality, practical items.</summary>
        Practical,

        /// <summary>Wants results in under 30 seconds.</summary>
        Impatient,

        /// <summary>Willing to wait up to 120 seconds.</summary>
        Patient,

        /// <summary>Prefers 1 item.  Equals RequestTag SingleItem.</summary>
        Minimalist,

        /// <summary>Prefers 1+ items.  Equals RequestTag MultipleItems.</summary>
        Maximalist,

        /// <summary>Prefers a variety of unique items, whether variety in type or group etc.  Equals RequestTag Variety.</summary>
        Diverse,

        /// <summary>Likes surprise items. Equals RequestTag Risky.</summary>
        Spontaneous,

        /// <summary>Prefers a specific group of items like Donut or Axe.  Equals RequestTag Group.</summary>
        Specific,

        /// <summary>Starts with fine happiness level.</summary>
        DebbyDowner,

        /// <summary>Starts at an upset happiness level.</summary>
        Bitter,

        /// <summary>Has a short but high range of acceptable happiness.</summary>
        Demanding,

        /// <summary>Has a wide range of acceptable happiness.</summary>
        Forgiving,

        /// <summary>Has a short and low range of acceptable happiness.</summary>
        Indifferent,

        /// <summary> Prefers to have their budget standard followed. Equals RequestType Efficient.</summary>
        Budgeter
    }

    public static class CustomerTypes
    {
        //private static readonly Dictionary<CustomerType, RequestTag> typeToTagMap =
        //    new Dictionary<CustomerType, RequestTag>
        //{
        //    { CustomerType.Picky, RequestTag.HighQuality },
        //    { CustomerType.Cheap, RequestTag.Cheap },
        //    { CustomerType.Impatient, RequestTag.Fast },
        //    { CustomerType.BigSpender, RequestTag.Expensive },
        //    { CustomerType.Maximalist, RequestTag.MultipleItems },
        //    { CustomerType.Minimalist, RequestTag.SingleItem },
        //};

        public static List<RequestTag> GetPreferredTags(Customer customer)
        {

            var tags = new HashSet<RequestTag>(); // avoids duplicates

            foreach (var type in customer.customerTypes)
            {
                switch (type)
                {
                    case CustomerType.Picky:
                        tags.Add(RequestTag.HighQuality);
                        break;

                    case CustomerType.Cheap:
                        tags.Add(RequestTag.Cheap);
                        break;

                    case CustomerType.Impatient:
                        tags.Add(RequestTag.Fast);
                        break;

                    case CustomerType.BigSpender:
                        tags.Add(RequestTag.Expensive);
                        break;

                    case CustomerType.Maximalist:
                        tags.Add(RequestTag.MultipleItems);
                        break;

                    case CustomerType.Minimalist:
                        tags.Add(RequestTag.SingleItem);
                        break;
                    case CustomerType.Patient:
                        tags.Add(RequestTag.Slow);
                        break;
                    case CustomerType.Diverse:
                        tags.Add(RequestTag.Variety);
                        break;
                    case CustomerType.Spontaneous:
                        tags.Add(RequestTag.Risky);
                        break;
                    case CustomerType.Practical:
                        tags.Add(RequestTag.MidQuality);
                        break;
                    case CustomerType.Specific:
                        tags.Add(RequestTag.Group);
                        break;
                    case CustomerType.Average:
                        tags.Add(RequestTag.MidRange);
                        break;
                    case CustomerType.Luxury:
                        tags.Add(RequestTag.HighRange);
                        break;
                    case CustomerType.Budgeter:
                        tags.Add(RequestTag.Efficient);
                        break;


                }

     
                //return customer.customerType switch
                //{
                //    CustomerType.Picky => new List<RequestTag> { RequestTag.HighQuality },
                //    CustomerType.Cheap => new List<RequestTag> { RequestTag.Cheap },
                //    CustomerType.Impatient => new List<RequestTag> { RequestTag.Fast },
                //    CustomerType.BigSpender => new List<RequestTag> { RequestTag.Expensive },
                //    CustomerType.Maximalist => new List<RequestTag> { RequestTag.MultipleItems },
                //    CustomerType.Minimalist => new List<RequestTag> { RequestTag.SingleItem },
                //    _ => new List<RequestTag>()
                //};
            }
            return tags.ToList();
        }
    }
    
}