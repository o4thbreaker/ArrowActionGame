using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButton; 
    [SerializeField] private Button continueButton; 
    [SerializeField] private Button settingsButton; 
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        newGameButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });

        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
