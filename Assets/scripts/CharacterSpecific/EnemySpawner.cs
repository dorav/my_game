using UnityEngine;
using AutomatedMovment;
using Assets.scripts;
using System;

public class EnemySpawner : MonoBehaviour
{
    public float WaveIndicatorShowTime = 5f;
    public UEnemyWaveProvider waveProvider;
    public ScoreManager Scorer;
    public Rigidbody2D Player;
    public WaveIndicator indicatorPrefab;

    EnemyWave activeWave;

    float RespawnCooldown = 0;
    int waveEnemiesToSpawn = 0;

    public float TimeBetweenWaves;
    private bool first = true;

    void Start ()
    {
    }

    void Update ()
    {
        if (waveEnemiesToSpawn == 0)
        {
            activeWave = waveProvider.Next();
            if (activeWave == null)
            {
                enabled = false;
                return;
            }

            StartNewWave();
        }

        RespawnCooldown -= Time.deltaTime;
        if (RespawnCooldown < 0 && waveEnemiesToSpawn > 0)
        {
            SpawnEnemy();

            RespawnCooldown = activeWave.RespawnTime;
            waveEnemiesToSpawn--;
        }
    }

    private void StartNewWave()
    {
        waveEnemiesToSpawn = activeWave.EnemiesToSpawn;
        setRespawnCooldown();

        spawnWaveIndicator();
    }

    private void setRespawnCooldown()
    {
        if (!first)
            RespawnCooldown = TimeBetweenWaves;
        else
            first = false;
    }

    private void spawnWaveIndicator()
    {
        Instantiate(indicatorPrefab).SetPath(activeWave.Path, 0);
        Instantiate(indicatorPrefab).SetPath(activeWave.Path, 1/3f);
        Instantiate(indicatorPrefab).SetPath(activeWave.Path, 2/3f);
    }

    public void SpawnEnemy()
    {
        var obj = Instantiate(activeWave.EnemyPrefab);
        obj.GetComponent<EnemyShooter>().SetConfig(this);
        obj.GetComponent<SplineWalker>().duration = activeWave.WaveFinishTime;
        obj.GetComponent<SplineWalker>().Spline = activeWave.Path;
        obj.GetComponent<SpriteRenderer>().transform.position = activeWave.Path.FirstPoint;
    }
}
