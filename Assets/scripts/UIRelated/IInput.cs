using UnityEngine;

public abstract class IInput : MonoBehaviour
{
    public abstract bool IsShooting();
    public abstract float GetHorizontalMovment();
}