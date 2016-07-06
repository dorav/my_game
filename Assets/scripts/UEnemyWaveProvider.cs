using UnityEngine;

namespace Assets.scripts
{
    [System.Serializable]
    public class EnemyWave
    {
        public int WaveFinishTime = 20;
        public int RemainingEnemiesToSpawn;
        public GameObject EnemyPrefab;
        public float RespawnTime;
    }

    public abstract class UEnemyWaveProvider : MonoBehaviour
    {
        public abstract EnemyWave Next();
    }
}
