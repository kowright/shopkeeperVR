using UnityEngine;
using System.Collections;
using TMPro;

namespace Assets.Scripts.Store
{
	public class Shelf: MonoBehaviour
	{
		[SerializeField] private ShelfTrigger TopShelfTrigger;
        [SerializeField] private ShelfTrigger MiddleShelfTrigger;
        [SerializeField] private ShelfTrigger BottomShelfTrigger;
        public TextMeshProUGUI AllShelvesCostText;


        // Use this for initialization
        void Start()
		{
			ProfitBoard.OnBusinessDayStarted += DayStarted;
            AllShelvesCostText.text = "Should this be all the shelves cost or something else?";
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
			if (BottomShelfTrigger != null)
			{
				BottomShelfTrigger.SetSpawnersToPaid();
			}
            if (TopShelfTrigger != null)
            {
                TopShelfTrigger.SetSpawnersToPaid();
            }
            if (MiddleShelfTrigger != null)
            {
                MiddleShelfTrigger.SetSpawnersToPaid();
            }
        }
	}
}