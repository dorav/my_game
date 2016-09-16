using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    public class MiltiplyEnemy : EnemyShooter
    {
        public EnemyShooter childPrefab;
        EnemySpawner spawner;

        public override void SetConfig(EnemySpawner enemySpawner)
        {
            base.SetConfig(enemySpawner);
            spawner = enemySpawner;
        }

        public override void Kill()
        {
            setAsLeftSpawn(InitSpawned());
            setAsRightSpawn(InitSpawned());
            base.Kill();
        }

        private void setAsLeftSpawn(SplineWalker child)
        {
            setAsRightSpawn(child);
            child.offset = new Vector3(-child.offset.x, child.offset.y, child.offset.z);
        }

        private void setAsRightSpawn(SplineWalker child)
        {
            var thisSize = GetComponent<SpriteRenderer>().bounds.size;
            var xOffset = thisSize.x / 2f;
            child.offset = new Vector3(xOffset, -thisSize.y, 0);
        }

        private SplineWalker InitSpawned()
        {
            // micro optimization
            var thisWalker = GetComponent<SplineWalker>();

            // Instantiating
            EnemyShooter obj = Instantiate(childPrefab);
            obj.GetComponent<EnemyShooter>().SetConfig(spawner);

            // setting common variables
            obj.transform.position = transform.position;

            var spawnedWalker = obj.GetComponent<SplineWalker>();
            spawnedWalker.duration = thisWalker.duration;
            spawnedWalker.progress = thisWalker.progress;
            spawnedWalker.Spline = GetComponent<SplineWalker>().Spline;

            UEnemyDestroyedListener.NumberOfActiveEnemies++;

            return spawnedWalker;
        }
    }
}
