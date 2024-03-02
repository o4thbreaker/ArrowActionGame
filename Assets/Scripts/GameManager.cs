using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum State 
    {
        InitialState,
        ControllingCharacter,
        ControllingArrow,
        RepeatingArrowPath,
        GameOver,
        LevelComplete
    }
    
    public Action OnArrowPathRepeated; 
    public Action OnArrowActivated; 
    public Action OnCharacterActivated; 
    public Action OnGameStart; 

    private State state;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateState(State.InitialState);
    }

    public void UpdateState(State newState)
    {
        state = newState;

        switch (newState)
        {
            case State.InitialState:
                HandleInitialState();
                break;
            case State.ControllingCharacter:
                HandleCharacterControlState();
                break;
            case State.ControllingArrow:
                HandleArrowRewindState();
                break;
            case State.RepeatingArrowPath:
                HandleArrowControlState();
                break;
            case State.GameOver:
                HandleGameOverState();
                break;
            case State.LevelComplete:
                HandleLevelCompleteState();
                break;
        }
    }

    private void HandleInitialState()
    {
        OnGameStart?.Invoke();
    }

    private void HandleCharacterControlState()
    {
        Debug.Log("In Character Control state");
        OnCharacterActivated?.Invoke();
    }

    private void HandleArrowRewindState()
    {
        OnArrowActivated?.Invoke();
    }

    private void HandleArrowControlState()
    {
        Debug.Log("In Arrow repeating its own path state");
        OnArrowPathRepeated?.Invoke();
    }

    private void HandleGameOverState()
    {

    }

    private void HandleLevelCompleteState()
    {

    }
}
