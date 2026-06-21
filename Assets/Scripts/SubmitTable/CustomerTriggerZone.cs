using Assets.Scripts.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SubmitTable
{
	public class CustomerTriggerZone: MonoBehaviour
	{
        public Customer currentCustomer;
        public CustomerComponent currentCustomerComponent;
        [SerializeField] private Station station;
        public static Action<CustomerComponent> OnCustomerTriggerEnter;
        //public static Action<CustomerComponent> OnCustomerTriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            var customer = other.GetComponent<CustomerComponent>();
            if (customer != null)
            {
                currentCustomer = customer.customer;
                currentCustomerComponent = customer;
                Debug.Log("Servicing: " + currentCustomer.customerName);
                //customer.StartPatienceTimer();
                OnCustomerTriggerEnter?.Invoke(customer);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var customer = other.GetComponent<CustomerComponent>();

            if (customer != null && currentCustomer != null)
            {
                Debug.Log("Bye: " + currentCustomer.customerName);
                currentCustomer = null;
                currentCustomerComponent = null;
                //OnCustomerTriggerExit?.Invoke(customer);

            }
        }
    }
}