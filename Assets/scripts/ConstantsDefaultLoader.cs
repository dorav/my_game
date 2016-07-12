using UnityEngine;
using System.Collections;

public class ConstantsDefaultLoader : MonoBehaviour
{
    public ParticleSystem hitEffectPrefab_;
    public ParticleSystem deathEffectPrefab_;

    public static ParticleSystem HitEffectPF;
    public static ParticleSystem DeathEffectPF;

    void Awake()
    {
        HitEffectPF = hitEffectPrefab_;
        DeathEffectPF = deathEffectPrefab_;
    }
}
