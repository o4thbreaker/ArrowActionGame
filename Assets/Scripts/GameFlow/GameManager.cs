using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum gameState 
    {
       GameStart,
       LevelEntered,
       Tutorial,
       LevelCompleted,
       GameCompleted,
       GameOver
    }
    
    public Action OnGameStart; 
    public Action OnGameCompleted; 
    public Action OnLevelEntered; 
    public Action OnTutorial;
    public Action OnLevelCompleted; 
    public Action OnGameOver;


    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private GameObject enemyPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateState(gameState.GameStart);
    }

    public void UpdateState(gameState newState)
    {

        switch (newState)
        {
            case gameState.GameStart:
                HandleGameStart();
                break;
            case gameState.LevelEntered:
                HandleLevelEntered();
                break;
            case gameState.Tutorial:
                HandleTutorial();
                break;
            case gameState.LevelCompleted:
                HandleLevelCompleted();
                break;
            case gameState.GameCompleted:
                HandleGameCompleted();
                break;
            case gameState.GameOver:
                HandleGameOver();
                break;
        }
    }

    private void HandleGameStart()
    {
        // TEMPORARY SOLUTION BECAUSE OF STRANGE BUG
        // TODO: FIX THIS BUG!!!!!!
        Time.timeScale = 1f; 

        OnGameStart?.Invoke();
    }

    private void HandleGameCompleted()
    {
        OnGameCompleted?.Invoke();
    }

    private void HandleTutorial()
    {
        OnTutorial?.Invoke();
    }

    private void HandleLevelEntered()
    {
        OnLevelEntered?.Invoke();

        StartCoroutine(EnemySpawnCoroutine());
    }

    private void HandleLevelCompleted()
    {
        OnLevelCompleted?.Invoke();

        Debug.Log($"<color=green>Level completed!</color>");
    }

    private void HandleGameOver()
    {
        OnGameOver?.Invoke();
    }

    private IEnumerator EnemySpawnCoroutine()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[i].position, enemySpawnPoints[i].rotation);
        }
    }
}
