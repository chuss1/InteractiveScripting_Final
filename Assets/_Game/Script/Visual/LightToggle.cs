using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToggle : MonoBehaviour {
    private PanelBehavior panel => GetComponentInParent<PanelBehavior>();
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;

    private void OnEnable() {
        panel.OnPanelActivated += ToggleLight;
        //Debug.Log(gameObject.name + " " + panel);
    }

    private void OnDestroy() {
        panel.OnPanelActivated -= ToggleLight;
    }

    private void ToggleLight(bool toggle) {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if(toggle) {
            mesh.material = greenMaterial;
        } else {
            mesh.material = redMaterial;
        }
    }
}
