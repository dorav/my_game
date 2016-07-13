using UnityEngine;
using System.Collections;

public class PlayerShieldScript : BasicCharacter
{
    public float shieldRemainingTime;
    public Vector3 maxSize;

    public override void Start ()
    {
        if (shieldRemainingTime > 0)
            Destroy(gameObject, shieldRemainingTime);
        base.Start();
        maxSize = transform.localScale;
    }

    float blinkTime = 2f;

    void Update ()
    {
        float normalizedTime = (Time.time % blinkTime) * Mathf.PI / blinkTime;
        var blinkPercentage = Mathf.Sin(normalizedTime);
        transform.localScale = Vector3.Lerp(maxSize * 1.1f, maxSize , blinkPercentage);
	}
}
