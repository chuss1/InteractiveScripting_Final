using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInput : MonoBehaviour {
    public Character_Input character;
    private Character_Input.MovementActions movement;
    private Character_Input.CameraLookActions cameraInput;
    private Character_Input.OtherActions otherActions;

    public Action<Vector2> horizontalMoveAction;
    public Action<float> sprintAction;
    public Action<float> crouchAction;
    public Action jumpAction;
    public Action fireWeapon;
    public Action interact;
    public Action dropItem;
    public Action useConsumable;
    public Action switchWeapon;

    private void Awake() {
        character = new Character_Input();
        movement = character.Movement;
        cameraInput = character.CameraLook;
        otherActions = character.Other;
    }

    private void OnEnable() {
        movement.Enable();
        cameraInput.Enable();
        otherActions.Enable();
    }

    private void OnDisable() {
        movement.Disable();
        cameraInput.Disable();
        otherActions.Disable();
    }

    private void Start() {
        movement.Jump.started += ctx => jumpAction?.Invoke();
        otherActions.FireWeapon.started += ctx => fireWeapon?.Invoke();
        otherActions.Interact.started += ctx => interact?.Invoke();
        otherActions.DropItem.started += ctx => dropItem?.Invoke();
        otherActions.ConsumeItem.started += ctx => useConsumable?.Invoke();
        otherActions.SwitchWeapon.started += ctx => switchWeapon?.Invoke();
    }

    private void Update() {
        Vector2 horizontal = movement.HorizontalMovement.ReadValue<Vector2>();
        horizontalMoveAction?.Invoke(horizontal);

        float crouching = movement.Crouch.ReadValue<float>();
        crouchAction?.Invoke(crouching);

        if(crouching == 0) {
            float sprinting = movement.Sprint.ReadValue<float>();
            sprintAction?.Invoke(sprinting);
        }
    }

}
