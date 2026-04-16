using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Customers.Rules
{
    public class RequestManager : MonoBehaviour
    {
        public RequestDatabase database;

        public CustomerRequest GetRequest(Customer customer, int day)
        {
            Debug.Log("GET REQUEST for " + customer.customerName);
            List<RequestTag> customerTags = CustomerTypes.GetPreferredTags(customer);
            Debug.Log("Tags to match " + customerTags);
            var validRequests = database.allRequests.FindAll(r =>
                r.difficulty <= GetTargetDifficulty(day) &&
                MatchesCustomer(r, customerTags, strictMatch: true)
            );

            if (validRequests.Count == 0)
            {
                Debug.LogWarning("No valid requests found, using soft match");
                var validRequestsSoft = database.allRequests.FindAll(r =>
                        r.difficulty <= GetTargetDifficulty(day) &&
                        MatchesCustomer(r, customerTags, strictMatch: false)
                );

                if (validRequestsSoft.Count == 0)
                {
                    Debug.LogWarning("No valid requests found with either matching system, using fallback");
                    return database.allRequests[0];
                }
                else
                {
                    Debug.Log(validRequestsSoft.Count + " to choose from");

                    int randomNumber = Random.Range(0, validRequestsSoft.Count);
                    Debug.Log("random number " + randomNumber);
                    return validRequestsSoft[randomNumber];

                }
            }
            else
            {
                Debug.Log(validRequests.Count + " to choose from");
                return validRequests[Random.Range(0, validRequests.Count)];

            }
        }

        private int GetTargetDifficulty(int day)
        {
            return Mathf.Clamp(day / 2, 1, 10);
        }

        private bool MatchesCustomer(CustomerRequest request, List<RequestTag> tags, bool strictMatch = false)
        {

            // Example: picky customers want high quality
            //if (customer.customerType == CustomerType.Picky)
            //    return request.tags.Contains(RequestTag.HighQuality);

            return strictMatch ? 
                request.Tags.All(tag => tags.Contains(tag)) : request.Tags.Any(tag => tags.Contains(tag));

            //if (strictMatch)
            //{
            //    return request.Tags.All(tag => tags.Contains(tag));

            //}

            //return request.Tags.Any(tag => tags.Contains(tag));
        }
    }
}