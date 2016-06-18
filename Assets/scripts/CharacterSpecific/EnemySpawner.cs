using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using AutomatedMovment;

public class EnemySpawner : MonoBehaviour
{
    public int WaveFinishTime = 10;
    public int RemainingEnemiesToSpawn;
    public int DefaultPathNumberOfSteps;
    public ScoreManager Scorer;
    public GameObject EnemyPrefab;
    public Rigidbody2D Player;
    public float RespawnTime;
    public float padTop;
    public float padBot;

    float RespawnCooldown = 0;

    public BezierSpline MovmentPath;


    void Start ()
    {
        MovmentPath = CreateDefaultPath(EnemyPrefab.GetComponent<SpriteRenderer>());
    }

    /*
     * The renderer is needed so the created object will 
     * be instantiated just above the screen
     */
    private BezierSpline CreateDefaultPath(Renderer renderer)
    {
        Vector3 appearingPos = CameraUtils.SpawnPointAboveView(renderer, 0);
        Vector3 pathStartPos = appearingPos;
        pathStartPos.y -= renderer.bounds.size.y;

        float stepHeight = -(Camera.main.ViewportToScreenPoint(new Vector3(0, (1 - padTop - padBot) / DefaultPathNumberOfSteps, 0)).y);
        float stepWidth = CameraUtils.SpawnPointAboveView(renderer, 1).x - appearingPos.x;

        var path = new BezierSpline(appearingPos, pathStartPos);

        for (int i = 0; i < DefaultPathNumberOfSteps; ++i)
        {
            var segmentStart = pathStartPos + new Vector3(0, stepHeight * 2) * i;
            var topRight = segmentStart + new Vector3(stepWidth, 0);
            var botRight = segmentStart + new Vector3(stepWidth, stepHeight);
            var topLeft = segmentStart + new Vector3(0, stepHeight);
            var botLeft = segmentStart + new Vector3(0, stepHeight * 2);

            path.AddPath(new StraightLineWalker(path.LastPoint, topRight));
            path.AddPath(new StraightLineWalker(path.LastPoint, botRight));
            path.AddPath(new StraightLineWalker(path.LastPoint, topLeft));
            path.AddPath(new StraightLineWalker(path.LastPoint, botLeft));
        }


        return path;
    }

    bool isRightSegment(int i)
    {
        return ((i - 1) / 2) % 2 == 0;
    }

    void Update ()
    {
        RespawnCooldown -= Time.deltaTime;
        if (RespawnCooldown < 0 && RemainingEnemiesToSpawn > 0)
        {
            var obj = Instantiate(EnemyPrefab);
            obj.GetComponent<EnemyShooter>().Player = Player;
            obj.GetComponent<EnemyShooter>().Scorer = Scorer;
            obj.GetComponent<SplineWalker>().duration = WaveFinishTime;
            obj.GetComponent<SplineWalker>().Spline = MovmentPath;
            obj.GetComponent<SpriteRenderer>().transform.position = MovmentPath.FirstPoint;

            RespawnCooldown = RespawnTime;
            RemainingEnemiesToSpawn--;
        }
	}
}
