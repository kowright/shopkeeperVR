using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Store
{
	public class Shelf: MonoBehaviour
	{
		[SerializeField] private ShelfTrigger trigger;


		// Use this for initialization
		void Start()
		{
			ProfitBoard.OnBusinessDayStarted += DayStarted;
		}

        private void OnDestroy()
        {
            ProfitBoard.OnBusinessDayStarted -= DayStarted;

        }
        // Update is called once per frame
        void Update()
		{

		}

		private void DayStarted()
		{
			if (trigger != null)
			{
				trigger.SetSpawnersToPaid();
			}
		}
	}
}