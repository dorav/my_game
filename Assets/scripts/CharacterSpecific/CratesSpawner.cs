using Assets.scripts.CharacterSpecific;
using UnityEngine;

public class CratesSpawner : UEnemyDestroyedListener
{
    public float BonusDropChance;
    public float CrateDropVelocity;
    public int MinCratesPerWave = 3;
    public GameObject[] CratePrefab;
    private int CratesReleasedInCurrentWave;

    void Start ()
    {
        UEnemyDestroyedListener.OnBeforeEnemyDestroyed += RandomizeBonusDrop;
        UNewWaveListener.OnNewWaveSpawned += NewWaveSpawned;
    }

    void OnDisable()
    {
        UEnemyDestroyedListener.OnBeforeEnemyDestroyed -= RandomizeBonusDrop;
        UNewWaveListener.OnNewWaveSpawned -= NewWaveSpawned;
    }

    private void NewWaveSpawned()
    {
        CratesReleasedInCurrentWave = 0;
    }

    private void RandomizeBonusDrop(EnemyShooter destroyed)
    {
        int potentialCrates = UEnemyDestroyedListener.NumberOfActiveEnemies;
        int cratesToRelease = MinCratesPerWave - CratesReleasedInCurrentWave;
        if (cratesToRelease == potentialCrates || shouldDropCrateByLuck())
            releaseCrate(destroyed);
    }

    private bool shouldDropCrateByLuck()
    {
        return Random.Range(0f, 1f) <= BonusDropChance;
    }

    private void releaseCrate(EnemyShooter destroyed)
    {
        var crate = Instantiate<GameObject>(CratePrefab[Random.Range(0, CratePrefab.Length)]);
        crate.transform.position = destroyed.transform.position;
        crate.GetComponent<Rigidbody2D>().velocity = new Vector2(0, CrateDropVelocity);

        ++CratesReleasedInCurrentWave;
    }
}
