using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.scripts;

public class SplitShotBonus : UCharacterInteractor
{
    public Weaponry weaponry;

    public class SplitShot : IWeapon
    {
        public float openingDegreeRad = 7.5f * Mathf.Deg2Rad;

        public SplitShot(IWeapon originalWeapon)
        {
            this.OriginalWeapon = originalWeapon;
        }

        public IWeapon OriginalWeapon { get; private set; }

        public List<GameCollider> shoot()
        {
            var shots = OriginalWeapon.shoot();
            shots.AddRange(OriginalWeapon.shoot());

            float angleDiff = openingDegreeRad * 2 / (shots.Count - 1); // doubled because the opening is to the two sides
            float angle = -openingDegreeRad;
            for (int i = 0; i < shots.Count; ++i)
            {
                ChangeAngle(shots[i].GetComponent<Rigidbody2D>(), angle);
                angle += angleDiff;
            }

            return shots;
        }

        private void ChangeAngle(Rigidbody2D origin, float destAngle)
        {
            var speed = origin.velocity.magnitude;
            origin.velocity = new Vector2(Mathf.Sin(destAngle), Mathf.Cos(destAngle)) * speed;
        }
    }


    void Start()
    {
        weaponry = PlayerShootButtonAction.Instance.weapons;
    }

    public override void InteractWith(PlayerScript player)
    {
        if (weaponry.SplitShotNumber < 8)
        {
            var splitShotNumber = weaponry.SplitShotNumber * 2;

            weaponry.setWeapon(new SplitShot(weaponry.currentWeapon));

            weaponry.SplitShotNumber = splitShotNumber;
            weaponry.DamageMultiplier /= splitShotNumber;
        }
        else
        {
            if (weaponry.DamageMultiplier < 1)
                weaponry.DamageMultiplier *= 2;
        }
    }
}
