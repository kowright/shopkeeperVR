using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.Store
{
	public class DayManager : MonoBehaviour
	{
        public static int day { get; private set; }
		public int rent => SetRent();

		/// <summary>
		/// Get time in seconds of how long the day countdown is
		/// </summary>
		public int dayTime => (day * 60) + 120; // 2 mins to start, every exyra day gives an extra minute

		//public int daytime => 30; //debug
        public static Action OnDayEnded;


		// each day will cost rent rent = day * rent * 2 ; rent = $25
		private int SetRent()
		{
			return day * 25 * 2;
		}

		public void SetNextDay()
		{
			day++;
		}

        // Use this for initialization
        void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}