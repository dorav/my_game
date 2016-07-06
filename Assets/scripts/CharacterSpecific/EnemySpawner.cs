using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using AutomatedMovment;

public class EnemySpawner : MonoBehaviour
{
    public int WaveFinishTime = 20;
    public int RemainingEnemiesToSpawn;
    public int DefaultPathNumberOfSteps;
    public ScoreManager Scorer;
    public GameObject EnemyPrefab;
    public Rigidbody2D Player;
    public float RespawnTime;
    public float padTop;
    public float padBot;

    float RespawnCooldown = 0;

    public BezierSpline zigZagPath;


    void Start ()
    {
        zigZagPath = CreateTopBotZigZagPath(EnemyPrefab.GetComponent<SpriteRenderer>());
    }

    private float getStepHeight()
    {
        var top = Camera.main.ViewportToWorldPoint(new Vector3(0, 1));
        var bot = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));

        var height = (top.y - bot.y) * (1 - padBot - padTop);
        var stepHeight = height / DefaultPathNumberOfSteps;

        return -stepHeight / 2;
    }

    /*
     * The renderer is needed so the created object will 
     * be instantiated just above the screen
     */
    private BezierSpline CreateTopBotZigZagPath(Renderer renderer)
    {
        Vector3 appearingPos = CameraUtils.SpawnPointAboveView(renderer, 0);
        Vector3 pathStartPos = appearingPos;
        pathStartPos.y -= renderer.bounds.size.y;

        float stepHeight = getStepHeight();
        float stepWidth = CameraUtils.SpawnPointAboveView(renderer, 1).x - appearingPos.x;

        // High speed for fast entrance
        float speed = 8;
        var path = new BezierSpline(new StraightLineWalker(appearingPos, pathStartPos, speed));

        for (int i = 0; i < DefaultPathNumberOfSteps; ++i)
        {
            var segmentStart = pathStartPos + new Vector3(0, stepHeight * 2 * i);
            var topRight = segmentStart + new Vector3(stepWidth, 0);
            var botRight = segmentStart + new Vector3(stepWidth, stepHeight);
            var topLeft = segmentStart + new Vector3(0, stepHeight);
            var botLeft = segmentStart + new Vector3(0, stepHeight * 2);

            path.AddPath(new StraightLineWalker(path.LastPoint, topRight, speed));
            // Fast entrance, but now we want to slow down
            speed = 2;
            path.AddPath(new StraightLineWalker(path.LastPoint, botRight, speed));
            path.AddPath(new StraightLineWalker(path.LastPoint, topLeft, speed));
            path.AddPath(new StraightLineWalker(path.LastPoint, botLeft, speed));
        }

        path.AddPath(new StraightLineWalker(path.LastPoint, 
                                            path.LastPoint + new Vector3(stepWidth, 0),
                                            8f));

        var endPoint = CameraUtils.SpawnPointBelowView(renderer, 1);
        path.AddPath(new StraightLineWalker(path.LastPoint, endPoint, 8f));

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
            obj.GetComponent<SplineWalker>().Spline = zigZagPath;
            obj.GetComponent<SpriteRenderer>().transform.position = zigZagPath.FirstPoint;

            RespawnCooldown = RespawnTime;
            RemainingEnemiesToSpawn--;
        }
	}
}
