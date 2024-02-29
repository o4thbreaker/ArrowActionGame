using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum State 
    {
        ControllingCharacter,
        ControllingArrow,
        GameOver,
        LevelComplete
    }
    
    public Action OnArrowActivated; 
    public Action OnCharacterActivated; 

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
        UpdateState(State.ControllingCharacter);
    }

    public void UpdateState(State newState)
    {
        state = newState;

        switch (newState)
        {
            case State.ControllingCharacter:
                HandleCharacterControlState();
                break;
            case State.ControllingArrow:
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

    private void HandleCharacterControlState()
    {
        Debug.Log("In Character Control state");
        OnCharacterActivated?.Invoke();
    }

    private void HandleArrowControlState()
    {
        Debug.Log("In Arrow Control state");
        OnArrowActivated?.Invoke();
    }

    private void HandleGameOverState()
    {

    }

    private void HandleLevelCompleteState()
    {

    }
}
