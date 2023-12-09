using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    private CharacterInput characterInput => GetComponent<CharacterInput>();
    private Rigidbody _rb;
    private Transform _groundCheckTransform;
    private Transform _characterOrientation;
    private float _downwardSpeedIncrement = 15.0f;
    private float _groundCheckDistance = 0.5f;
    private float _previousHeight;
    private float _originalSpeed;
    private bool _isGrounded;
#region  Movement Settings
    [Header("Movement Settings")]
    [Tooltip("The base speed of your character")]
    [SerializeField] private float moveSpeed;
    
    [Tooltip("A modifier that multiplies your base Move Speed")]
    [SerializeField] private float walkSpeedModifier;
    
    [Tooltip("A modifier that will multiply your move speed when you are sprinting")]
    [SerializeField] private float sprintSpeedModifier;
    
    [Tooltip("A modifier that will multiply your move speed when you are crouching.")]
    [SerializeField] private float crouchSpeedModifier;
#endregion

#region  Jump Settings
    [Header("Jump Settings")]
    [Tooltip("This is the Power for your jump")]
    [SerializeField] private float jumpForce;
    [Tooltip("Include all the layers that you want your character to jump off of")]
    [SerializeField] private LayerMask groundLayer;
#endregion

#region Crouch Settings
    [Header("Crouch Settings")]
    [Tooltip("This variable will set your players scale to be what you set it to when you crouch")]
    [SerializeField] private Vector3 crouchHeight;
    [Tooltip("This variable will set your players scale to be what you set it to when your standing")]
    [SerializeField] private Vector3 standHeight;
#endregion
    
    private void OnEnable() {
        characterInput.horizontalMoveAction += HorizontalMovement;
        characterInput.sprintAction += Sprint;
        characterInput.crouchAction += Crouch;
        characterInput.jumpAction += Jump;
    }

    private void OnDisable() {
        characterInput.horizontalMoveAction -= HorizontalMovement;
        characterInput.sprintAction -= Sprint;
        characterInput.crouchAction -= Crouch;
        characterInput.jumpAction -= Jump;
    }

    private void Awake() {
        TryGetComponent<Rigidbody>(out _rb);
        _groundCheckTransform = transform.GetChild(0);
        _characterOrientation = transform.GetChild(1);
        _originalSpeed = moveSpeed;
    }

    private void Update() {
        // Perform the ground check using a raycast.
        _isGrounded = Physics.OverlapSphere(_groundCheckTransform.position, _groundCheckDistance, groundLayer).Length > 0;

        if(!_isGrounded) {
            float currentHeight = transform.position.y;
            
            if(currentHeight < _previousHeight) {
                //we are falling downward
                Debug.Log("We are falling down");
                _rb.velocity += Vector3.down * _downwardSpeedIncrement * Time.deltaTime;
            }
            _previousHeight = currentHeight;
        }
    }


    private void HorizontalMovement(Vector2 horizontalInput) {

        Vector3 move = _characterOrientation.forward * horizontalInput.y + _characterOrientation.right * horizontalInput.x;
        
        move *= moveSpeed;

        _rb.velocity = new Vector3(move.x, _rb.velocity.y, move.z);
    }
    
    private void Sprint(float sprintFloat) {
        //move *= inputManager.characterInput.Movement.Sprint.ReadValue<float>() == 0 ? speed : speed * runSpeedModifier;
        if(sprintFloat != 0 && _isGrounded) {
            moveSpeed = _originalSpeed * sprintSpeedModifier;
        } else {
            moveSpeed = _originalSpeed * walkSpeedModifier;
        }
    }

    private void Crouch(float crouchFloat) {
        if(crouchFloat != 0 && _isGrounded) {
            transform.localScale = crouchHeight;
            moveSpeed = _originalSpeed * crouchSpeedModifier;
        } else {
            transform.localScale = standHeight;
            moveSpeed = _originalSpeed * walkSpeedModifier;
        }
    }

    private void Jump() {
        if(_isGrounded) {
            _rb.AddForce(Vector3.up * jumpForce);
        }
    } 

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckDistance);
    // }

}
