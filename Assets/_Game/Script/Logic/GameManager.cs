using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public IDamagable characterHealth;

    private bool isKeySpawned;
    [SerializeField] private GameObject endGameKeycard;
    [SerializeField] private Transform keyCardSpawn;
    [SerializeField] private Canvas inGameUI;
    [SerializeField] private Canvas gameOverScreen;
    public List<BaseAI> EnemiesLeft = new List<BaseAI>();
    
    private ScoreTracker scoreTracker => GetComponent<ScoreTracker>();
    private void Awake() {
        characterHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<IDamagable>();
        if(characterHealth == null) {
            Debug.LogError("CharacterHealth is not found");}

        BaseAI[] foundEnemies = FindObjectsOfType<BaseAI>();
        EnemiesLeft.AddRange(foundEnemies);
        isKeySpawned = false;
        gameOverScreen.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(true);
    }

    private void Update() {
        if(EnemiesLeft.Count == 0 && !isKeySpawned) {
            Debug.Log("All enemies defeated.");
            GameObject finalKeycard = Instantiate(endGameKeycard);
            finalKeycard.transform.SetParent(keyCardSpawn);
            finalKeycard.transform.localPosition = new Vector3(0,0,0);
            isKeySpawned = true;
        }
    }

    private void OnEnable() {
        characterHealth.OnDeath += StopGame;
    }

    private void OnDisable() {
        characterHealth.OnDeath -= StopGame;
    }


    private void StopGame(Vector3 position) {
        Debug.Log("Stop Game!");
        Time.timeScale = 0;
        inGameUI.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void AddEnemyToList(BaseAI enemy)
    {
        EnemiesLeft.Add(enemy);
    }
    
    public void RemoveEnemy(BaseAI enemy) {
        scoreTracker.AddScore();
        EnemiesLeft.Remove(enemy);
    }

}
