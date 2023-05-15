using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Timeline;

namespace World
{
    [CreateAssetMenu(fileName = "World Info", menuName = "World/Info")]
    public class WorldInfo : ScriptableObject
    {
        [Header("Base Info")]
        public string WorldName = "World";


        [Header("Cycle duration")]
        public float cycleDuration = 24f;
        public float durationDayHour = 12f;
        public float durationNightHour = 12f;
        public float currentTime = 0;
        public float2 NightStart;
        public float2 DayStart;
        [Space(20)]
        public CurrentTimeDay currentTimeDay;
        [Space(20)]
        [Header("Day/Night")]
        public float durationCycleIRTM = 24f;
        [Tooltip("Time of a day in minutes real time")]
        public float durationDayIRTM = 12f;
        public float durationNightIRTM = 12f;
        [Tooltip("The number of hours In Game that is a day")]
        public float durationDayHours = 12f;
        public float numberOfMinuteIRTMPerHour;
        public float durationTickMinute;
        public float durationDaySeconds;
        public float durationHourSeconds;
        [Space(2)]
        public float tick;
        [Space(10)]
        public float2 ActualTime;

        private void OnValidate()
        {
            Calculate();
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void Calculate()
        {
            durationNightIRTM = durationCycleIRTM - durationDayIRTM;
            durationDayIRTM = durationCycleIRTM - durationNightIRTM;

            durationDaySeconds = durationDayIRTM * 60f; 

            numberOfMinuteIRTMPerHour = durationDayIRTM / durationDayHours;
            durationTickMinute =numberOfMinuteIRTMPerHour / 60f;

            durationHourSeconds = numberOfMinuteIRTMPerHour * 60f;
            tick = durationHourSeconds / 60f;
        }

        public KeyValuePair<bool,CurrentTimeDay> AdvanceTime(CurrentTimeDay current)
        {
            ActualTime.y++;
            if (ActualTime.y == 60f)
            {
                ActualTime.x++;
                ActualTime.y = 0f;
                if(ActualTime.Equals(DayStart))
                {
                    return new(true, CurrentTimeDay.Day);
                }
                else if (ActualTime.Equals(NightStart))
                {
                    return new(true, CurrentTimeDay.Night);
                }
            }
            if (ActualTime.x == cycleDuration)
            {
                ActualTime.x = 0;
                return new(true, current);
            }
            return new(false, current);
        }
    }

    public enum CurrentTimeDay { Day,Night }
}