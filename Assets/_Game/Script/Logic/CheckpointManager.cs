using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public Transform defaultSpawn;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        LoadCheckpoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ResetCheckpoint();
        }
    }

    void LoadCheckpoint()
    {
        Debug.Log("Current Scene is " + SceneManager.GetActiveScene().buildIndex);
        float xPos, yPos, zPos;

        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1)) {
            xPos = defaultSpawn.position.x;
            yPos = defaultSpawn.position.y;
            zPos = defaultSpawn.position.z;
        } else {
            xPos = PlayerPrefs.GetFloat("xPos");
            yPos = PlayerPrefs.GetFloat("yPos");
            zPos = PlayerPrefs.GetFloat("zPos");
        }

        // Check if the PlayerPrefs values are zero (not set) and set them to the default spawn if they are.
        if (xPos == 0f && yPos == 0f && zPos == 0f)
        {
            xPos = defaultSpawn.position.x;
            yPos = defaultSpawn.position.y;
            zPos = defaultSpawn.position.z;
        }

        Vector3 tempSpawn = new Vector3(xPos, yPos, zPos);
        player.transform.position = tempSpawn;
    }

    void ResetCheckpoint()
    {
        Scene currentScene  = SceneManager.GetActiveScene();
        
        if(currentScene == SceneManager.GetSceneByBuildIndex(1)) {
            PlayerPrefs.SetFloat("xPos", defaultSpawn.position.x);
            PlayerPrefs.SetFloat("yPos", defaultSpawn.position.y);
            PlayerPrefs.SetFloat("zPos", defaultSpawn.position.z);
        } else {
            SceneManager.LoadScene(1);
            PlayerPrefs.SetFloat("xPos", defaultSpawn.position.x);
            PlayerPrefs.SetFloat("yPos", defaultSpawn.position.y);
            PlayerPrefs.SetFloat("zPos", defaultSpawn.position.z);

        }


        LoadCheckpoint();
    }
}
