using Assets.Scripts.Customers.Rules;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Customers
{
	public enum CustomerType
	{
		Cheap, // under $20
		Average, // under $100
		BigSpender,
        Picky,
        Impatient, // under 30 seconds
        Patient, // under 120 seconds
        Minimalist,
        Maximalist,
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