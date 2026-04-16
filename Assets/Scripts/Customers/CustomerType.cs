using Assets.Scripts.Customers.Rules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers
{
	public enum CustomerType
	{
		Cheap,
		Average,
		BigSpender,
        Picky,
        Impatient, 
        Minimalist,
        Maximalist,
	}

    public static class CustomerTypes
    {
        public static List<RequestTag> GetPreferredTags(Customer customer)
        {
            return customer.customerType switch
            {
                CustomerType.Picky => new List<RequestTag> { RequestTag.HighQuality },
                CustomerType.Cheap => new List<RequestTag> { RequestTag.Cheap },
                CustomerType.Impatient => new List<RequestTag> { RequestTag.Fast },
                CustomerType.BigSpender => new List<RequestTag> { RequestTag.Expensive },
                CustomerType.Maximalist => new List<RequestTag> { RequestTag.MultipleItems },
                CustomerType.Minimalist => new List<RequestTag> { RequestTag.SingleItem },
                _ => new List<RequestTag>()
            };
        }
    }
    
}