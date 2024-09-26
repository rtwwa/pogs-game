using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndManager : MonoBehaviour
{
    [SerializeField]
    public GameObject gameOverMenu; // Панель с меню
    public TextMeshProUGUI finalScoreText;

    public static GameEndManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Проверка: назначен ли объект панели
            if (gameOverMenu != null)
            {
                Debug.Log("Панель назначена, скрываем ее.");
                gameOverMenu.SetActive(false); // Скрываем панель
            }
            else
            {
                Debug.LogError("gameOverMenu не назначена в инспекторе!");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndGame()
    {
        Debug.Log("Игра завершена!");

        // Показываем меню
        if (gameOverMenu != null)
        {
            Debug.Log("Показываем панель.");
            finalScoreText.text = "Фишки игрока: " + Player.Instance.MyCaps.Count.ToString() + "\nФишки бота: " + Bot.Instance.MyCaps.Count.ToString();
            gameOverMenu.SetActive(true); // Показываем панель
        }
        else
        {
            Debug.LogError("gameOverMenu не назначена в инспекторе!");
        }
    }
}
