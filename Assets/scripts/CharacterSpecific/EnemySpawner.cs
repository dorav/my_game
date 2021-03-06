﻿using UnityEngine;
using Assets.scripts;
using UnityEngine.UI;
using System.Collections;
using Assets.scripts.CharacterSpecific;

public class EnemySpawner : MonoBehaviour
{
    public float WaveIndicatorShowTime = 5f;
    public UEnemyWaveProvider waveProvider;
    public Text activeEnemiesPresenter;
    public Rigidbody2D Player;
    public WaveIndicator indicatorPrefab;

    private EnemyWave ActiveWave;

    float RespawnCooldown = 0;
    int waveEnemiesToSpawn = 0;

    public float TimeBetweenWaves;
    private bool first = true;
    private bool spawning = true;

    void Start ()
    {
        UEnemyDestroyedListener.setNumberOfEnemies(0);
    }
    int count = 0;
    void Update ()
    {
        if (spawning == false)
            return;

        if (UEnemyDestroyedListener.NumberOfActiveEnemies == 0)
        {
            ActiveWave = waveProvider.Next();
            if (ActiveWave == null)
            {
                enabled = false;
                return;
            }
            StartCoroutine(StartNewWave());
            spawning = false;
            return;
        }

        RespawnCooldown -= Time.deltaTime;
        if (RespawnCooldown < 0 && waveEnemiesToSpawn > 0)
        {
            SpawnEnemy();

            RespawnCooldown = ActiveWave.RespawnTime;
            waveEnemiesToSpawn--;
        }
    }

    private IEnumerator StartNewWave()
    {
        yield return new WaitForSeconds(TimeBetweenWaves);
        waveEnemiesToSpawn = ActiveWave.EnemiesToSpawn;
        UEnemyDestroyedListener.setNumberOfEnemies(waveEnemiesToSpawn);
        setRespawnCooldown();

        spawnWaveIndicator();
        spawning = true;
        UNewWaveListener.ReportNewWave();
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

    public EnemyWave getActiveWave()
    {
        return ActiveWave;
    }
}
