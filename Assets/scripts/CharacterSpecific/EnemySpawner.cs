using UnityEngine;
using AutomatedMovment;
using Assets.scripts;

public class EnemySpawner : MonoBehaviour
{
    public UEnemyWaveProvider waveProvider;
    EnemyWave activeWave;
    public int DefaultPathNumberOfSteps;
    public float padTop;
    public float padBot;
    public ScoreManager Scorer;
    public Rigidbody2D Player;

    float RespawnCooldown = 0;

    public BezierSpline zigZagPath;
    public float TimeBetweenWaves;

    void Start ()
    {
        activeWave = waveProvider.Next();
        zigZagPath = CreateTopBotZigZagPath(activeWave.EnemyPrefab.GetComponent<SpriteRenderer>());
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
        if (activeWave == null)
            return;

        if (activeWave.RemainingEnemiesToSpawn == 0)
        {
            activeWave = waveProvider.Next();
            if (activeWave == null)
                return;
            RespawnCooldown = TimeBetweenWaves;
        }

        RespawnCooldown -= Time.deltaTime;
        if (RespawnCooldown < 0 && activeWave.RemainingEnemiesToSpawn > 0)
        {
            SpawnEnemy();

            RespawnCooldown = activeWave.RespawnTime;
            activeWave.RemainingEnemiesToSpawn--;
        }
    }

    public void SpawnEnemy()
    {
        var obj = Instantiate(activeWave.EnemyPrefab);
        obj.GetComponent<EnemyShooter>().SetConfig(this);
        obj.GetComponent<SplineWalker>().duration = activeWave.WaveFinishTime;
        obj.GetComponent<SplineWalker>().Spline = zigZagPath;
        obj.GetComponent<SpriteRenderer>().transform.position = zigZagPath.FirstPoint;
    }
}
