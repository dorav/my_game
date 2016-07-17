using System.Collections;
using UnityEngine;

public class WaveIndicator : SplineWalker
{
    public float loopTime = 2f;
    public float indicatorLifeTime = 5f;

    LineRenderer line;
    public Color startColor = Color.magenta;
    Color startColorTransp;
    public Color endColor = Color.red;
    Color endColorTransp;

    public void Start()
    {
        startColorTransp = startColor;
        startColorTransp.a = 0;

        endColorTransp = endColor;
        endColorTransp.a = 0;
    }

    public override void FixedUpdate ()
    {
        var currentStartColor = Color.Lerp(startColor, startColorTransp, progress);
        var currentEndColor = Color.Lerp(endColor, endColorTransp, progress);
        line.SetColors(currentStartColor, currentEndColor);

        if (progress == 1f && mode == SplineWalkerMode.Once)
        {
            Destroy(gameObject);
            return;
        }

        base.FixedUpdate();
    }

    internal void SetPath(BezierSpline path, float progress)
    {
        Spline = path;
        this.progress = progress;
        duration = loopTime;
        offset = Vector3.zero;
        transform.position = Vector3.zero;
        resetPositions();
        line = GetComponent<LineRenderer>();
        line.SetWidth(6, 4);
        mode = SplineWalkerMode.Loop;
        StartCoroutine(StopLooping());
    }

    IEnumerator StopLooping()
    {
        yield return new WaitForSeconds(indicatorLifeTime);

        mode = SplineWalkerMode.Once;
    }

    private void resetPositions()
    {
        for (int i = 0; i < prevPositions.Length; ++i)
            prevPositions[i] = Spline.GetPoint(progress);
    }

    Vector3[] prevPositions = new Vector3[10];

    public override void LoopWalker()
    {
        base.LoopWalker();
        resetPositions();
    }

    public override void SetPosition(Vector3 newPos)
    {
        for (int i = 0; i < prevPositions.Length - 1; ++i)
            prevPositions[i] = prevPositions[i+1];

        prevPositions[prevPositions.Length - 1] = newPos;

        line = GetComponent<LineRenderer>();
        line.SetVertexCount(prevPositions.Length);
        line.SetPositions(prevPositions);
    }
}
