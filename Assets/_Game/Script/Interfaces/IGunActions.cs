using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGunActions {
    public void Spawn(Transform parent);
    public void Shoot();
    public IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit);
    public TrailRenderer CreateTrail();
    public IEnumerator ResetCooldown();
}
