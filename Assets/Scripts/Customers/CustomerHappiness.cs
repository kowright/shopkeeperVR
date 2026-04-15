using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Customers
{

        public enum CustomerHappiness
        {
            Happy,
            Fine,
            Upset,
        }

    public class CustomerHappinessManager
    {
        private float fineHappinessLowThresold;
        private float fineHappinessHighThresold;

        public CustomerHappinessManager(float lowFineThreshold, float highFineThreshold)
        {
            fineHappinessLowThresold = lowFineThreshold;
            fineHappinessHighThresold = highFineThreshold;
        }
        public CustomerHappiness GetHapinessFromPatience(float percentage)
        {

            if (percentage < fineHappinessLowThresold)
            {
                return CustomerHappiness.Upset;
            }

            else if (percentage > fineHappinessHighThresold)
            {
                return CustomerHappiness.Happy;
            }

            return CustomerHappiness.Fine;

        }
    }
}