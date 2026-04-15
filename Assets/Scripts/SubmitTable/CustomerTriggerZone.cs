using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SubmitTable
{
	public class CustomerTriggerZone: MonoBehaviour
	{
        public Customer currentCustomer;


        private void OnTriggerEnter(Collider other)
        {
            var customer = other.GetComponent<CustomerComponent>();
            if (customer != null)
            {
                currentCustomer = customer.customer;
                Debug.Log("Servicing: " + currentCustomer.customerName);
                //turn on request ask
                customer.StartPatienceTimer();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var customer = other.GetComponent<CustomerComponent>();

            if (customer != null && currentCustomer != null)
            {
                Debug.Log("Bye: " + currentCustomer.customerName);
                currentCustomer = null;
            }
        }
    }
}