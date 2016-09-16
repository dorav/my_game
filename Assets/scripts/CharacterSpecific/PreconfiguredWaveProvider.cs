using UnityEngine;
using System.Collections;
using Assets.scripts;
using System;
using AutomatedMovment;

public class PreconfiguredWaveProvider : UEnemyWaveProvider
{
    public EnemyWave[] waves;
    public int DefaultPathNumberOfSteps = 2;
    public float padTop = 0f;
    public float padBot = 0.3f;

    int currentWave = 0;

    public override EnemyWave Next()
    {
        if (currentWave < waves.Length)
        {
            var wave = waves[currentWave++];
            wave.Path = CreateTopBotZigZagPath(wave.EnemyPrefab.GetComponent<Renderer>());
            return wave;
        }
        else
            return null;
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

    private float getStepHeight()
    {
        var top = Camera.main.ViewportToWorldPoint(new Vector3(0, 1));
        var bot = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));

        var height = (top.y - bot.y) * (1 - padBot - padTop);
        var stepHeight = height / DefaultPathNumberOfSteps;

        return -stepHeight / 2;
    }

    bool isRightSegment(int i)
    {
        return ((i - 1) / 2) % 2 == 0;
    }
}
