using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointCollision : MonoBehaviour
{
    public bool checkPointHit = false;
    private CheckpointManager cpManager => GameObject.Find("GameManager").GetComponent<CheckpointManager>();

    private void Start() {
        checkPointHit = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Checkpoint1"))
        {
            checkPointHit = true;
            Debug.Log("Reached Checkpoint 1");

            SceneManager.LoadScene(2);

            PlayerPrefs.SetFloat("xPos", other.gameObject.transform.position.x);
            PlayerPrefs.SetFloat("yPos", other.gameObject.transform.position.y);
            PlayerPrefs.SetFloat("zPos", other.gameObject.transform.position.z);
        } 
    }
}
