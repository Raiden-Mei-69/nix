using System;
using System.Collections;
using UnityEngine;

namespace Farm.Day
{
    public class DayManager : MonoBehaviour
    {
        public DayInfo dayInfo;

        public static DayManager instance;

        private void Awake()
        {
            instance = this;
            StartCoroutine(MovingDay());
        }

        public void NextDay()
        {
            dayInfo.Day++;
        }

        public IEnumerator MovingDay()
        {
            do
            {
                yield return new WaitForSeconds(dayInfo.DayDuration);
                NextDay();
            } while (true);
        }
    }

    [Serializable]
    public class DayInfo
    {
        public int Day = 1;

        public float TimeOfDay = 0f;
        public float DayDuration = 120f;
    }
}
