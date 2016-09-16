using UnityEngine;
using System.Collections;

public class GrowingShrinkingObject : MonoBehaviour
{
    Vector3 maxSize;
    public float ScaleCoeficient = 1.3f;
    public float blinkTime = 2f;

    public void Start()
    {
        maxSize = transform.localScale;
    }


    void Update()
    {
        float normalizedTime = ((Time.time % blinkTime) / blinkTime) * Mathf.PI;
        var blinkPercentage = Mathf.Sin(normalizedTime);
        transform.localScale = Vector3.Lerp(maxSize * ScaleCoeficient, maxSize, blinkPercentage);
    }
}
