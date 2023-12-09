using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerAI : BaseAI {
    [SerializeField] private Transform RunFromTarget;
    [SerializeField] private float stoppingDistance;

    private void Update() {
        if(CheckForPlayer() <= _aiConfig.ProximityToPlayer) {
            RunFromPlayer();
        } else {
            _rb.velocity = Vector3.zero;
        }
    }
    
    void RunFromPlayer()
    {
        if(Vector3.Distance(transform.position, RunFromTarget.position) > stoppingDistance)
        {
            transform.LookAt(RunFromTarget);
            _rb.AddForce(transform.forward * _aiConfig.AISpeed * Time.deltaTime);
        }
        else
        {
            // Stop the AI by setting its velocity to zero
            _rb.velocity = Vector3.zero;
        }
    }

}
