using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.scripts
{
    public class Weaponry : MonoBehaviour
    {
        private float damageMultiplier = 1f;
        public float DamageMultiplier
        {
            get
            {
                return damageMultiplier;
            }
            set
            {
                damageMultiplier = value;
                if (OnWeaponChange != null)
                    OnWeaponChange();
            }
        }

        internal int MaxMultiplier = 8;
        internal float MinMultiplier = 0.125f;

        public IWeapon baseWeapon;
        public IWeapon wrappingWeapon;
        public SortedList<float, GameCollider> ShotPrefabsByDamage = new SortedList<float, GameCollider>();

        public delegate void WeaponChanged();
        public event WeaponChanged OnWeaponChange;

        void Awake()
        {
            baseWeapon = new RegularShot(this, ConstantsDefaultLoader.PlayerBulletPrefabs);
            wrappingWeapon = new NoChangeWrapper(this);
        }

        public void Shoot()
        {
            foreach (var collider in wrappingWeapon.shoot())
            {
                var pos = transform.position;
                pos.y += 100;
                collider.transform.position = pos;
                collider.Damage *= DamageMultiplier;
            }
        }
    }

    class RegularShot : IWeapon
    {
        public GameCollider[] shots;
        private Weaponry weaponry;

        public RegularShot(Weaponry weaponry, GameCollider[] prefab)
        {
            this.weaponry = weaponry;
            shots = prefab;
            int max = 0;
            for (int coef = (int)Mathf.Log(weaponry.MinMultiplier, 2); coef < 1; ++coef, ++max)
                weaponry.ShotPrefabsByDamage[Mathf.Pow(2, coef)] = shots[max];

            for (int coef = 1; Mathf.Pow(2, coef) <= weaponry.MaxMultiplier; ++coef, ++max)
                weaponry.ShotPrefabsByDamage[Mathf.Pow(2, coef)] = shots[max];
        }

        public List<GameCollider> shoot()
        {
            GameCollider shotPrefab = weaponry.ShotPrefabsByDamage.EqualOrNextGreater(weaponry.DamageMultiplier);
            var latest = MonoBehaviour.Instantiate(shotPrefab);
            var collider = latest.GetComponent<GameCollider>();
            latest.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1000f);

            return new List<GameCollider> { collider };
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
