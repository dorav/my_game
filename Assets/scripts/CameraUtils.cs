using UnityEngine;
using System.Collections;

public class CameraUtils
{
    public static Vector3 RandomSpawnPointAboveView(Renderer renderer)
    {
        return TransformToJustAboveViewport(renderer.bounds, null, false);
    }

    public static Vector3 RandomSpawnPointAboveViewClamped(Renderer renderer)
    {
        return TransformToJustAboveViewport(renderer.bounds, null, true);
    }

    public static Vector3 SpawnPointAboveView(Renderer renderer, float screenPortX)
    {
        return TransformToJustAboveViewport(renderer.bounds, screenPortX, true);
    }

    public static Vector3 TransformToJustAboveViewport(Bounds worldLocation, float? screenPortX, bool clamped)
    {
        var lowerLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        var upperRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));

        float halfWidth = worldLocation.size.x / 2;
        float halfHeight = worldLocation.size.y / 2;

        float y = upperRight.y + halfHeight;
        float x;
        if (screenPortX.HasValue)
            x = screenPortX.Value * (upperRight.x - lowerLeft.x) + lowerLeft.x;
        else
            x = Random.Range(lowerLeft.x, upperRight.x);

        if (clamped)
            x = Mathf.Clamp(x, lowerLeft.x + halfWidth, upperRight.x - halfWidth);

        return new Vector3(x, y, 100);
    }
}
