using UnityEngine;

namespace AutomatedMovment
{
    public interface IPath
    {
        Vector3 LastPoint { get; }
        Vector3 FirstPoint { get; }

        Vector3 GetPoint(float relativeTime);
        float Distance { get; }
        float Speed { get; }
    }
}