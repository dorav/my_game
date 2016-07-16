﻿using UnityEngine;
using AutomatedMovment;
using Assets.scripts;
using System;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public float WaveIndicatorShowTime = 5f;
    public UEnemyWaveProvider waveProvider;
    public ScoreManager Scorer;
    public Text activeEnemiesPresenter;
    public Rigidbody2D Player;
    public WaveIndicator indicatorPrefab;

    public EnemyWave ActiveWave;

    float RespawnCooldown = 0;
    int waveEnemiesToSpawn = 0;

    public float TimeBetweenWaves;
    private bool first = true;

    void Start ()
    {
    }

    void Update ()
    {
        if (Scorer.NumberOfActiveEnemies == 0)
        {
            ActiveWave = waveProvider.Next();
            if (ActiveWave == null)
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

            RespawnCooldown = ActiveWave.RespawnTime;
            waveEnemiesToSpawn--;
        }
    }

    private void StartNewWave()
    {
        waveEnemiesToSpawn = ActiveWave.EnemiesToSpawn;
        Scorer.NumberOfActiveEnemies = waveEnemiesToSpawn;
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
        Instantiate(indicatorPrefab).SetPath(ActiveWave.Path, 0);
        Instantiate(indicatorPrefab).SetPath(ActiveWave.Path, 1/3f);
        Instantiate(indicatorPrefab).SetPath(ActiveWave.Path, 2/3f);
    }

    public void SpawnEnemy()
    {
        var obj = Instantiate(ActiveWave.EnemyPrefab);
        obj.GetComponent<EnemyShooter>().SetConfig(this);
        obj.GetComponent<SplineWalker>().duration = ActiveWave.WaveFinishTime;
        obj.GetComponent<SplineWalker>().Spline = ActiveWave.Path;
        obj.GetComponent<SpriteRenderer>().transform.position = ActiveWave.Path.FirstPoint;
    }
}