using UnityEngine;
using System;
using Assets.scripts;

public class PlayerShootButtonAction : MonoBehaviour
{
    public PlayerScript Player;
    public InputAdapter input;
    public float ShotCooldown = 0.45f;

    public bool ShotReady { get { return remainingShotCooldownInSeconds <= 0;  } }
    float remainingShotCooldownInSeconds = 0;

    static PlayerShootButtonAction instance;

    public static PlayerShootButtonAction Instance
    {
        get
        {
            return instance;
        }
        set
        {
            if (Instance != null)
                throw new Exception("Can't Instantiate two instances of this class");

            instance = value;
        }
    }

    void Awake()
    {
    }

    private void Shoot()
    {
        if (ShotReady)
        {
            Player.weaponry.Shoot();
            remainingShotCooldownInSeconds = ShotCooldown;
        }
    }

	public void Update ()
    {
        if (Player == null)
            return;

        remainingShotCooldownInSeconds -= Time.deltaTime;

	    if (input.IsShooting())
            Shoot();
	}
}
