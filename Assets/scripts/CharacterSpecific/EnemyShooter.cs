using UnityEngine;
using System.Collections;
using System;
using Assets.scripts;

public class EnemyShooter : BasicCharacter
{
    public Rigidbody2D Player;
    public GameObject bulletPrefab;
    public ScoreManager Scorer;
    public float BulletSpeed;
    public float ShotDelay;
    public int ScoreValue;

    static float lastShotTime = Time.fixedTime;

    public EnemyWave RelatedWave { get; private set; }

    void Update ()
    {
        if (shouldShoot())
        { 
            lastShotTime = Time.realtimeSinceStartup;

            GameObject bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -BulletSpeed);
            bullet.GetComponent<Rigidbody2D>().transform.position = new Vector3(transform.position.x, transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y, 100);
        }
	}

    public override void Kill()
    {
        Scorer.EnemyDestroyed(this);
        base.Kill();
    }

    private bool shouldShoot()
    {
        return isApproximatlyAbovePlayer() && (Time.realtimeSinceStartup - lastShotTime >= ShotDelay);
    }

    private bool isApproximatlyAbovePlayer()
    {
        if (Player == null)
            return false;

        return Mathf.Abs(Player.position.x - GetComponent<Transform>().position.x) < 10f;
    }

    public virtual void SetConfig(EnemySpawner enemySpawner)
    {
        Player = enemySpawner.Player;
        Scorer = enemySpawner.Scorer;
        RelatedWave = enemySpawner.ActiveWave;
    }
}
