using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Store
{
	public class DayManager
	{
        public static int day { get; private set; } = 1;
		public int rent => SetRent();

		/// <summary>
		/// Get time in seconds of how long the day countdown is
		/// </summary>
		public int dayTime => ((day-1) * 60) + 180; // 2 mins to start, every extra day gives an extra minute

        public static float daySpawnRate => getDaySpawnRate();

		//public int daytime => 30; //debug
        public static Action OnDayEnded;
        private static Dictionary<int, float> daySpawnRateDict = new Dictionary<int, float> {
            { 1, 20 }, { 2, 15 }, { 3, 20 }, {4, 20 }, {5, 20 }, {6, 20 }, {7, 20 }, {8, 20 }, {9, 20 }, {10, 20 } };

        // each day will cost rent rent = day * rent * 2 ; rent = $25
        private int SetRent()
		{
			return day * 25 * 2;
		}

		public void SetNextDay()
		{
			day++;
		}

        private static float getDaySpawnRate()
        {
            if (daySpawnRateDict.TryGetValue(day, out float rate))
            {
                return rate;
            }

            return 20f;
        }

        // Day Plan

        // DAY 1: 
        // - description: people will want low-mid tier fruit 
        // - customer type: 1 type customers, no picky, cheap
        // - items: +apples & bananas
        // - quality: low-good
        // - requests: certain quality, max items, unique items available

        // DAY 2:
        // - description: people will want higher tier food
        // - customer type: 1-2 type customers, average
        // - items: +donuts
        // - quality: all qualities food
        // - requests: no duplicate items, certain item group

        // DAY 3:
        // - description: people will want basic accessories
        // - customer type: 2 type customers
        // - items: +accessories
        // - quality: low-good 
        // - store: can buy a second shelf 

        // DAY 4:
        // - description: people will want higher tier accesories & might be in a bad mood
        // - customer type: 2 type customers, bitter customers come
        // - items: 
        // - quality: all qualities accessories
        // - request: surprise me quality, surprise me group

        // DAY 5: 
        // - description: people know about the store! some people might have money to blow and people want weapons
        // - customer type: 2-3 type customers, big spenders come
        // - items: weapons
        // - quality: low-good 
        // - requests: no duplicate types, surprise me type available

        // DAY 6:
        // - description: everyone is demanding
        // - customer type: 2-3 types
        // - items: all qualities weapons
        // - requests: fast service
        // - store: can buy a third shelf 

        // DAY 7:
        // - description: people want various types of items for their quests! 
        // - customer type: 3 types
        // - requests: description quests available
        // - quality: low-good
        // - store: can buy a fourth shelf 

        // DAY 8:
        // - dscription: 
        // - customer type: 3-4 types
        // - quality: all qualities
        // - store: can buy a fifth shelf 

        // DAY 9:
        // - description: 
        // - customer type: 4 types

        // DAY 10:
        // - description: 
        // - customer type: 4+ types
    }
}