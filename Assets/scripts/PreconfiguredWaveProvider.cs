using UnityEngine;
using System.Collections;
using Assets.scripts;
using System;

public class PreconfiguredWaveProvider : UEnemyWaveProvider
{
    public EnemyWave[] waves;
    int currentWave = 0;

    public override EnemyWave Next()
    {
        if (currentWave < waves.Length)
            return waves[currentWave++];
        else
            return null;
    }
}
