using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.scripts
{
    public class Weaponry
    {
        public float DamageMultiplier = 1f;
        public int MaxMultiplier = 8;
        internal float MinMultiplier = 0.125f;

        public IWeapon baseWeapon;
        public IWeapon wrappingWeapon;
        public PlayerScript Player;

        public Weaponry(PlayerScript player)
        {
            Player = player;
            baseWeapon = new RegularShot(this, ConstantsDefaultLoader.PlayerBulletPrefab);
            wrappingWeapon = new NoChangeWrapper(this);
        }

        public void Shoot()
        {
            foreach (var collider in wrappingWeapon.shoot())
            {
                var pos = Player.transform.position;
                pos.y += 100;
                collider.transform.position = pos;
                collider.Damage *= DamageMultiplier;
            }
        }

        internal void setWeapon(IWeapon weapon)
        {
            baseWeapon = weapon;
        }
    }

    class RegularShot : IWeapon
    {
        public GameCollider[] shots;
        private Weaponry weaponry;
        private SortedDictionary<float, GameCollider> dmgMultToPrefab = new SortedDictionary<float, GameCollider>();

        public RegularShot(Weaponry weaponry, GameCollider[] prefab)
        {
            this.weaponry = weaponry;
            shots = prefab;
            int max = 0;
            for (int coef = (int)Mathf.Log(weaponry.MinMultiplier, 2); coef < 1; ++coef, ++max)
                dmgMultToPrefab[Mathf.Pow(2, coef)] = shots[max];

            for (int coef = 1; Mathf.Pow(2, coef) <= weaponry.MaxMultiplier; ++coef, ++max)
                dmgMultToPrefab[Mathf.Pow(2, coef)] = shots[max];
        }

        public List<GameCollider> shoot()
        {
            var latest = MonoBehaviour.Instantiate(shotPrefab().gameObject);
            var collider = latest.GetComponent<GameCollider>();
            latest.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1000f);

            return new List<GameCollider> { collider };
        }

        private GameCollider shotPrefab()
        {
            var shotPrefab = shots[0];
            foreach (KeyValuePair<float, GameCollider> shot in dmgMultToPrefab)
            {
                if (shot.Key > weaponry.DamageMultiplier)
                    return shotPrefab;

                shotPrefab = shot.Value;
            }

            return shotPrefab;
        }
    }

    class NoChangeWrapper : IWeapon
    {
        Weaponry weaponry;

        public NoChangeWrapper(Weaponry weaponry)
        {
            this.weaponry = weaponry;
        }

        public List<GameCollider> shoot()
        {
            return weaponry.baseWeapon.shoot();
        }
    }
}
