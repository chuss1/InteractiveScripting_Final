using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private float lookInterval = 0.1f;
    [Range(30,110)]
    [SerializeField] private float fieldOfView = 75;
    private Transform emitter;
    private GameObject[] playerObj;

    void Start()
    {
        emitter = this.transform.GetChild(0);
        playerObj = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(CheckForPlayerObj());
    }

    IEnumerator CheckForPlayerObj() 
    {
        while(true) 
        {
            yield return new WaitForSeconds(lookInterval);

            foreach(GameObject user in playerObj) // check for enemies
            {
                Ray ray = new Ray(emitter.position, user.transform.position - emitter.position); // draw a ray from the emitter to enemy
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 100)) 
                {
                    if(hit.transform.gameObject.CompareTag("Player")) 
                    {
                        Vector3 targetDir = user.transform.position - emitter.position;
                        float angle = Vector3.Angle(targetDir, emitter.forward);

                        if (angle < fieldOfView) 
                        {
                            Debug.Log("Found player.");
                            Debug.DrawRay(emitter.position, user.transform.position - emitter.position, Color.green, 4);
                        } 
                        else 
                        {
                            Debug.DrawRay(emitter.position, user.transform.position - emitter.position, Color.yellow, 4);
                        }
                        
                    } 
                    else 
                    {
                        Debug.DrawRay(emitter.position, user.transform.position - emitter.position, Color.red, 4);
                    }

                }
            }
        }
    }
}