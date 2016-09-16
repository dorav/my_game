using UnityEngine;

namespace Assets.scripts
{
    [System.Serializable]
    public class EnemyWave
    {
        public int WaveFinishTime = 20;
        public int EnemiesToSpawn;
        public GameObject EnemyPrefab;
        public float RespawnTime;

        public BezierSpline Path { get; set; }
    }

    public abstract class UEnemyWaveProvider : MonoBehaviour
    {
        public abstract EnemyWave Next();
    }
}
