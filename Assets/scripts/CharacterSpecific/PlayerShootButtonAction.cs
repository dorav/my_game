using UnityEngine;
using System.Collections;
using System;

public class PlayerShootButtonAction : MonoBehaviour
{
    public GameObject BulletRegular;
    public Rigidbody2D Player;
    public InputAdapter input;
    public float ShotCooldown = 0.45f;

    public bool ShotReady { get { return remainingShotCooldownInSeconds <= 0;  } }
    float remainingShotCooldownInSeconds = 0;

    private void Shoot()
    {
        if (ShotReady)
        {
            var pos = Player.transform.position;
            pos.y += 100;
            var latest = (GameObject)Instantiate(BulletRegular, pos, new Quaternion());
            latest.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1000f);

            remainingShotCooldownInSeconds = ShotCooldown;
        }
    }


	void Update ()
    {
        if (Player == null)
            return;

        remainingShotCooldownInSeconds -= Time.deltaTime;

	    if (input.IsShooting())
            Shoot();
	}
}
