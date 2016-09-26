using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.scripts;

public class SplitShotBonus : UCharacterInteractor
{
    public Weaponry weaponry;
    private int MaxShotMultiplier = 8;

    public class SplitShot : IWeapon
    {
        public float openingDegreeRad = 7.5f * Mathf.Deg2Rad;
        public int ShotMultiplier = 2;
        private Weaponry weaponry;

        public SplitShot(Weaponry weaponry)
        {
            this.weaponry = weaponry;
        }

        public List<GameCollider> shoot()
        {
            List<GameCollider> shots = new List<GameCollider>();
            for (int i = 0; i < ShotMultiplier; ++i)
                shots.AddRange(weaponry.baseWeapon.shoot());

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
        var splitShot = weaponry.wrappingWeapon as SplitShot;
        if (splitShot != null)
        {
            if (splitShot.ShotMultiplier * 2 > MaxShotMultiplier)
                return;

            splitShot.ShotMultiplier *= 2;
        }
        else
            weaponry.wrappingWeapon = new SplitShot(weaponry);

        weaponry.DamageMultiplier /= 2;
    }
}
