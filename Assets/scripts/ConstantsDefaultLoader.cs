using UnityEngine;
using System.Collections;

public class ConstantsDefaultLoader : MonoBehaviour
{
    public ParticleSystem hitEffectPrefab_;
    public ParticleSystem deathEffectPrefab_;
    public PlayerShieldScript playerShieldPrefab_;

    public static ParticleSystem HitEffectPF;
    public static ParticleSystem DeathEffectPF;
    public static PlayerShieldScript PlayerShieldPrefab;

    void Awake()
    {
        HitEffectPF = hitEffectPrefab_;
        DeathEffectPF = deathEffectPrefab_;
        PlayerShieldPrefab = playerShieldPrefab_;
    }
}
