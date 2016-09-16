using UnityEngine;
using System.Collections;

public class CratesSpawner : UEnemyDestroyedListener
{
    public float BonusDropChance;
    public float CrateDropVelocity;
    public GameObject CratePrefab;

	void Start ()
    {
        UEnemyDestroyedListener.OnEnemyDestroyed += RandomizeBonusDrop;
    }

    void OnDisable()
    {
        UEnemyDestroyedListener.OnEnemyDestroyed -= RandomizeBonusDrop;
    }

    private void RandomizeBonusDrop(EnemyShooter destroyed)
    {
        float dropRoll = Random.Range(0f, 1f);
        if (dropRoll <= BonusDropChance)
        {
            var crate = Instantiate<GameObject>(CratePrefab);
            crate.transform.position = destroyed.transform.position;
            crate.GetComponent<Rigidbody2D>().velocity = new Vector2(0, CrateDropVelocity);
        }
    }
}
