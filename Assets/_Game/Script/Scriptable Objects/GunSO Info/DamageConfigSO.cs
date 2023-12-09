using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "Damage Config", menuName = "Guns/Damage Config", order = 1)]
public class DamageConfigSO : ScriptableObject {
    public MinMaxCurve DamageCurve = new MinMaxCurve(1, new AnimationCurve(), new AnimationCurve());
    public int MaxAmmo;
    public float reloadSpeed;

    
    private void Reset() {
        DamageCurve.mode = ParticleSystemCurveMode.Curve;
    }

    public float GetDamage(float Distace = 0) {
        return Mathf.Ceil(DamageCurve.Evaluate(Distace, Random.value));
    }
}
