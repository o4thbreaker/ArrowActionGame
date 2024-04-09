using System;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance { get; private set; }

    public enum playerState 
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

    private playerState state;

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
        UpdateState(playerState.InitialState);
    }

    public void UpdateState(playerState newState)
    {
        state = newState;

        switch (newState)
        {
            case playerState.InitialState:
                HandleInitialState();
                break;
            case playerState.ControllingCharacter:
                HandleCharacterControlState();
                break;
            case playerState.ControllingArrow:
                HandleArrowRewindState();
                break;
            case playerState.RepeatingArrowPath:
                HandleArrowControlState();
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
}
