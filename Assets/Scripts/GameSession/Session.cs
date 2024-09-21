using UnityEngine;
using System.Collections;

public class Session : MonoBehaviour
{
    public Player player;
    public Bot bot;

    private bool isPlayerTurn = true;  // ����������, ��� ������ ���

    void Start()
    {
        // �������� ���� � ���� ������
        StartTurn();
    }

    void StartTurn()
    {
        if (isPlayerTurn)
        {
            // ����� ������ ������� ���� ���
            Debug.Log("Player's turn");
        }
        else
        {
            // ��� ����
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

        // ��������� ������� �� �����������
        yield return new WaitForSeconds(1.0f);

        // ������ ���� ����
        bot.MakeMove();

        // ����� ���� ���� �������� ��� ������
        EndTurn();
    }
}
