using UnityEngine;
using System.Collections;

public class Session : MonoBehaviour
{
    public Player player;
    public Bot bot;

    private bool isPlayerTurn = true;  // Определяет, чей сейчас ход

    void Start()
    {
        // Начинаем игру с хода игрока
        StartTurn();
    }

    void StartTurn()
    {
        if (isPlayerTurn)
        {
            // Игрок должен сделать свой ход
            Debug.Log("Player's turn");
        }
        else
        {
            // Ход бота
            StartCoroutine(BotTurn());
        }
    }

    public void EndTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        StartTurn();
    }

    IEnumerator BotTurn()
    {
        Debug.Log("Bot is thinking...");

        // Симуляция времени на обдумывание
        yield return new WaitForSeconds(1.0f);

        // Логика хода бота
        bot.MakeMove();

        // После хода бота передаем ход игроку
        EndTurn();
    }
}
