using System;
using UnityEngine;

namespace Assets.scripts.CharacterSpecific
{
    public class UNewWaveListener : MonoBehaviour
    {
        public delegate void NewWaveEvent();
        public static event NewWaveEvent OnNewWaveSpawned;

        public static void ReportNewWave()
        {
            if (OnNewWaveSpawned != null)
                OnNewWaveSpawned();
        }
    }
}