using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    [CreateAssetMenu(menuName = "Database/RequestDatabase")]
    public class RequestDatabase : ScriptableObject
    {
        public List<CustomerRequest> allRequests;
    }
}