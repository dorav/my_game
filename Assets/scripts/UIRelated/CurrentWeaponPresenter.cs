using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine.UI;

public class CurrentWeaponPresenter : MonoBehaviour
{
    private const float SemiTransparentAlpha = 146f / 255f;
    private const float SemiTransparentV = 117f / 255f;

    private SortedList<float, SpriteRenderer> displayedWeaps = new SortedList<float, SpriteRenderer>();
    private SpriteRenderer currentHighlight;
    public Text damageText; 
    public PlayerScript player;

    void Start()
    {
        player.weaponry.OnWeaponChange += OnWeaponChange;
        SetDisplayedWeaps(player.weaponry.ShotPrefabsByDamage);
        currentHighlight = displayedWeaps.EqualOrNextGreater(0);
        OnWeaponChange();
    }

    public void SetDisplayedWeaps(SortedList<float, GameCollider> prefabs)
    {
        float x = -20f;
        foreach (var shotPrefab in prefabs)
        {
            displayedWeaps[shotPrefab.Key] = createNewWeap(x, shotPrefab.Value);
            x += 30;
        }
    }

    void OnDisable()
    {
        player.weaponry.OnWeaponChange -= OnWeaponChange;
    }

    private void OnWeaponChange()
    {
        var shouldHightlight = displayedWeaps.EqualOrNextGreater(player.weaponry.DamageMultiplier);

        if (shouldHightlight != currentHighlight)
        {
            setSemiTransparrent(currentHighlight);
            currentHighlight = shouldHightlight;
            setNonTransparent(currentHighlight);
        }
    }

    private SpriteRenderer createNewWeap(float x, GameCollider shotPrefab)
    {
        var shot = Instantiate(shotPrefab);
        shot.transform.parent = transform;
        shot.transform.localPosition = new Vector2(x, 0);
        shot.GetComponent<BoxCollider2D>().enabled = false;
        return setSemiTransparrent(shot.GetComponent<SpriteRenderer>());
    }

    private static SpriteRenderer setSemiTransparrent(SpriteRenderer shot)
    {
        //float h, s, v;
        //Color.RGBToHSV(shot.color, out h, out s, 117);

        //var newColor = Color.HSVToRGB(h, s, v);
        var newColor = shot.color;
        newColor.a = SemiTransparentAlpha;
        shot.color = newColor;
        return shot;
    }

    void Update()
    {
        damageText.text = player.weaponry.DamageMultiplier.ToString();
    }

    private void setNonTransparent(SpriteRenderer sprite)
    {
        var nonTransparentColor = sprite.color;
        nonTransparentColor.a = 1;
        sprite.color = nonTransparentColor;
    }
}
