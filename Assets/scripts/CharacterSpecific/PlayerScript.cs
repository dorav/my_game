﻿using UnityEngine;
using System.Collections;
using Assets.scripts;
using UnityEngine.SceneManagement;

public class PlayerScript : BasicCharacter
{
    public PlayerHealthBar healthBar;
    public CurrentWeaponPresenter weaponPresenter;

    PlayerShieldScript shield;

    public int SplitShotMultiplier { get; internal set; }
    public Weaponry weaponry { get; private set; }

    public void SetSpeed(float speedX)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, 0);
    }

    // Update is called once per frame
    void Update()
    {
        BoundPlayerMovmentToScreen();
    }

    public override void Start()
    {
        base.Start();
        base.SouldIgnoreCollisions = false;
        healthBar.MaxHealth = Health;
        healthBar.UpdateHealth(Health);
        spawnShield();
        weaponry = GetComponent<Weaponry>();
    }

    void OnDisable()
    {
        SceneManager.LoadScene("OpeningScreen");
    }

    public override void TakeHitFrom(GameCollider dmgDealer)
    {
        var interactor = dmgDealer.GetComponent<UCharacterInteractor>();
        if (interactor != null)
            interactor.InteractWith(this);
        else
        {
            base.TakeHitFrom(dmgDealer);
            healthBar.UpdateHealth(Health);

            spawnShield();
        }
    }

    private void spawnShield()
    {
        if (shield == null)
        {
            shield = Instantiate(ConstantsDefaultLoader.PlayerShieldPrefab);
            shield.transform.parent = transform;
            shield.transform.localPosition = new Vector3(0, 0, 101);
            shield.Health = float.PositiveInfinity;
        }
    }

    public Rect BoundsToScreenRect(Bounds bounds)
    {
        // Get mesh origin and farthest extent (this works best with simple convex meshes)
        Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
        Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));

        // Create rect in screen space and return - does not account for camera perspective
        return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
    }


    void BoundPlayerMovmentToScreen()
    {
        const float PADDING_COEF = 1.5f;
        var camera = Camera.main;
        var bottomLeft = camera.ScreenToWorldPoint(Vector3.zero);
        var topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        var realSize = GetComponent<Renderer>().bounds;
        var playerHalfWidth = (BoundsToScreenRect(realSize).size.x / 2) * PADDING_COEF;

        var minX = bottomLeft.x + playerHalfWidth;
        var maxX = topRight.x - playerHalfWidth;

        var newX = Mathf.Clamp(transform.position.x, minX, maxX);

        transform.position = new Vector3(
             newX,
             transform.position.y,
             transform.position.z);
    }
}
