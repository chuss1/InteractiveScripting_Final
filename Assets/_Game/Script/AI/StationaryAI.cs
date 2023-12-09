using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryAI : BaseAI {
    [SerializeField] private GunSelector gunSelector;
    private Quaternion initialRotation;
    private Vector3 initialTransform;
    [SerializeField] private float rotationSpeed;

    private void Start() {
        initialTransform = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update() {
        if(CheckForPlayer() <= _aiConfig.ProximityToPlayer) {
            isPlayerInRange = true;
            SetTarget(_playerTarget.transform.position);
        } else {
            isPlayerInRange = false;
            transform.rotation = initialRotation;
            transform.position = initialTransform;
        }
    }

    private void SetTarget(Vector3 targetPosition) {
        // Calculate an offset to make the AI look higher (e.g., towards the player's chest or head)
        Vector3 offset = new Vector3(0f, 1.0f, 0f); // You can adjust the Y-component as needed

        // Calculate the final target position by adding the offset to the player's position
        Vector3 adjustedTargetPosition = targetPosition + offset;
        // Rotate the AI's forward vector to point towards the adjusted target position
        Quaternion targetRotation = Quaternion.LookRotation(adjustedTargetPosition - transform.position);

        // Smoothly interpolate the AI's rotation when looking at the player
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Check if the player is within the distance for using the ability
        float distanceToPlayer = Vector3.Distance(transform.position, adjustedTargetPosition);
        if (distanceToPlayer <= _aiConfig.DistanceForAbility) {
            FireWeapon();
            Debug.Log("firingweapon");
        }
    }

    private void FireWeapon() {
        if (gunSelector.ActiveGun != null) {
            Debug.Log("Active Gun Not Null");
            gunSelector.ActiveGun.Shoot();
        } else {
            Debug.LogError("ActiveGun is null in StationaryAI.FireWeapon");
        }
    }


}
