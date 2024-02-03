using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
    }
    public void TryAgainButton()
    {
        SceneManager.LoadScene("steampunk");
    }
}
