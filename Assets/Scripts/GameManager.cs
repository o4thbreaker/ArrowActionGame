using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI killText;
    public int totalHeads;
    public int goalHeads;

    public int currentEnemies;
    public int goalEnemies;

    private void Start()
    {
        UpdateCount();
    }

    private void Goal()
    {
        if (totalHeads == goalHeads || currentEnemies == goalEnemies)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    private void OnEnable()
    {
        HitBox.OnHeadshot += OnCollectibleCollected;
        HitBox.OnKill += HitBox_OnKill;
    }

    private void HitBox_OnKill()
    {
        currentEnemies++;
        UpdateCount();
    }

    private void OnDisable()
    {
        HitBox.OnHeadshot -= OnCollectibleCollected;
        HitBox.OnKill -= HitBox_OnKill;
    }

    private void OnCollectibleCollected()
    {
        totalHeads++;
        UpdateCount();
    }

    private void UpdateCount()
    {
        countText.text = $"Всего поражений в голову: {totalHeads} / {goalHeads}";
        killText.text = $"Всего поражено врагов: {currentEnemies} / {goalEnemies}";
    }

    private void Update()
    {
        Goal();
    }


}
