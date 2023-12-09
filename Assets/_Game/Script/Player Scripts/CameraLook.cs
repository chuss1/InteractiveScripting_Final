using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour {
    [SerializeField]private CharacterInput characterInput;
    [SerializeField]private Transform characterBody;
    [SerializeField] private Transform characterOrientation;
    [SerializeField] private float mouseSensitivity;

    private float xRotation = 0;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate() {
        UpdateCamera();
    }

    private void UpdateCamera() {

        Vector2 mouseXYDelta = characterInput.character.CameraLook.Look.ReadValue<Vector2>()
         * mouseSensitivity * Time.deltaTime;

         xRotation -= mouseXYDelta.y;
         xRotation = Mathf.Clamp(xRotation, -90f, 90f);

         characterOrientation.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
         //characterBody.localRotation = Quaternion.Euler(xRotation, mouseXYDelta.x, 0);
         characterBody.Rotate(Vector3.up * mouseXYDelta.x);

         //Debug.Log(mouseX + " " + mouseY);
         //Debug.Log(mouseXYDelta);

    }
}
