using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
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
            foreach (RequestTag tag in customerTags)
            {
                Debug.Log("Preferred tag: " + tag);
            }

            var validRequests = database.allRequests.FindAll(r =>
                r.difficulty == day &&
                MatchesCustomer(r, customerTags, strictMatch: true)

            );

            //foreach (var r in database.allRequests)
            //{
            //    bool difficultyPass = r.difficulty <= day;
            //    bool tagPass = MatchesCustomer(r, customerTags, true);

            //    Debug.Log(
            //        $"{r.name} | diff={r.difficulty} pass={difficultyPass} | tags={tagPass}"
            //    );
            //}

            //foreach (CustomerRequest r in database.allRequests)
            //{
        
            //    foreach(RequestTag rt in r.Tags)
            //    {
            //        if(r.difficulty == 2)
            //        {
            //            Debug.Log("request tags " + r.name);
            //            Debug.Log("tag: " + rt);
            //        }
            //    }
            //}

            if (validRequests.Count == 0)
            {
                Debug.LogWarning("No valid requests found for " + customer.customerName +", using soft match");
                var validRequestsSoft = database.allRequests.FindAll(r =>
                        r.difficulty == day &&
                        MatchesCustomer(r, customerTags, strictMatch: false)

                );

                if (validRequestsSoft.Count == 0)
                {
                    Debug.LogWarning("No valid requests found with either matching system, using fallback");
                    return database.allRequests[0];
                }
                else
                {
                    Debug.Log(validRequestsSoft.Count + " to choose from [NOT STRICT]");

                    int randomNumber = Random.Range(0, validRequestsSoft.Count);
                
                    return validRequestsSoft[randomNumber];

                }
            }
            else
            {
                Debug.Log(validRequests.Count + " to choose from [STRICT]");
                return validRequests[Random.Range(0, validRequests.Count)];

            }
        }

        private bool MatchesCustomer(CustomerRequest request, List<RequestTag> tags, bool strictMatch = false)
        {

            // Example: picky customers want high quality
            //if (customer.customerType == CustomerType.Picky)

            //    return request.tags.Contains(RequestTag.HighQuality);

            //Debug.Log("Customer tags: " + string.Join(", ", tags));
            //Debug.Log("Request tags: " + string.Join(", ", request.Tags));
            if (request.Tags == null || request.Tags.Count == 0)
                return false;

            return strictMatch ?
                 tags.All(tag => request.Tags.Contains(tag)) : request.Tags.Any(tag => tags.Contains(tag));
  

            //if (strictMatch)
            //{
            //    return request.Tags.All(tag => tags.Contains(tag));

            //}

            //return request.Tags.Any(tag => tags.Contains(tag));
        }
    }
}