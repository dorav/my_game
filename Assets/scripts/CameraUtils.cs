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

    public static Vector3 SpawnPointBelowView(Renderer renderer, float screenPortX)
    {
        Vector3 lowerLeft = GetWorldLowerLeft();
        Vector3 upperRight = GetWorldUpperRight();

        var bounds = renderer.bounds;

        float y = GetWorldYBelow(lowerLeft, bounds.size.y);
        float x = GetWorldX(lowerLeft, upperRight, bounds.size.x, true, screenPortX);

        return new Vector3(x, y, 100);
    }

    public static Vector3 SpawnPointAboveView(Renderer renderer, float screenPortX)
    {
        return TransformToJustAboveViewport(renderer.bounds, screenPortX, true);
    }

    private static Vector3 TransformToJustAboveViewport(Bounds objectToPlace, float? screenPortX, bool clamped)
    {
        Vector3 lowerLeft = GetWorldLowerLeft();
        Vector3 upperRight = GetWorldUpperRight();

        float y = GetWorldYAbove(upperRight, objectToPlace.size.y);
        float x = GetWorldX(lowerLeft, upperRight, objectToPlace.size.x, clamped, screenPortX);

        return new Vector3(x, y, 100);
    }

    private static Vector3 GetWorldUpperRight()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(1, 1));
    }

    private static Vector3 GetWorldLowerLeft()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
    }

    private static float GetWorldX(Vector3 lowerLeft, Vector3 upperRight, float width, bool clamped, float? screenPortX)
    {
        float x;
        if (screenPortX.HasValue)
            x = screenPortX.Value * (upperRight.x - lowerLeft.x) + lowerLeft.x;
        else
            x = Random.Range(lowerLeft.x, upperRight.x);

        var halfWidth = width / 2f;

        if (clamped)
            x = Mathf.Clamp(x, lowerLeft.x + halfWidth, upperRight.x - halfWidth);
        return x;
    }

    private static float GetWorldYAbove(Vector3 upperRight, float height)
    {
        return upperRight.y + height / 2;
    }

    private static float GetWorldYBelow(Vector3 upperRight, float height)
    {
        return upperRight.y - height / 2;
    }
}
