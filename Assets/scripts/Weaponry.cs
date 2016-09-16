using UnityEngine;
using System.Collections.Generic;

namespace Assets.scripts
{
    public class Weaponry
    {
        public int SplitShotNumber = 1;
        public float DamageMultiplier = 1f;

        public IWeapon currentWeapon;
        public Rigidbody2D Player;

        public Weaponry(Rigidbody2D player)
        {
            Player = player;
            currentWeapon = new RegularShot(ConstantsDefaultLoader.PlayerBulletPrefab);
        }

        public void Shoot()
        {
            foreach (var collider in currentWeapon.shoot())
            {
                var pos = Player.transform.position;
                pos.y += 100;
                collider.transform.position = pos;
                collider.Damage *= DamageMultiplier;
            }
        }

        internal void setWeapon(IWeapon weapon)
        {
            DamageMultiplier *= SplitShotNumber;
            SplitShotNumber = 1;
            currentWeapon = weapon;
        }
    }

    class RegularShot : IWeapon
    {
        public RegularShot(GameCollider prefab)
        {
            shot = prefab;
        }
        public GameCollider shot;
        public List<GameCollider> shoot()
        {
            var latest = MonoBehaviour.Instantiate(shot.gameObject);
            var collider = latest.GetComponent<GameCollider>();
            latest.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1000f);

            return new List<GameCollider> { collider };
        }
    }
}
