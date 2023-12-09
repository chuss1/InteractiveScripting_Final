using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehavior : MonoBehaviour {
    public void ThrowGrenade() {
        Debug.Log("Throwing " + gameObject.name);
        Destroy(this.gameObject);
    }
}
