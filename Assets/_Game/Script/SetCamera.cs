using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamera : MonoBehaviour {
    [SerializeField] private Transform cameraPos;

    private void Update() {
        transform.position = cameraPos.position;
        transform.rotation = cameraPos.rotation;
    }
}
