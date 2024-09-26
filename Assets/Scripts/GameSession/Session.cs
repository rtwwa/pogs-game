using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Session : MonoBehaviour
{
    public static Session Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private Bot bot;

    [SerializeField] private CapSpawner CapSpawner;

    public bool IsPlayerTurn { get; set; } = true;  // ����������, ��� ������ ���

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����� ��������� ������ ��� �������� ����� �����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // ����������� ������� ���������� ����
        CapMover.Instance.OnMoveCompleted += EndTurn;

        // �������� ���� � ���� ������
        StartTurn();
    }

    void StartTurn()
    {
        if (IsPlayerTurn)
        {
            Debug.Log("Player's turn");
        }
        else
        {
            StartCoroutine(BotTurn());
        }
    }

    public void EndTurn()
    {
        if (CapSpawner.Instance.SpawnedCaps.Count == 0)
        {
            GameEndManager.Instance.EndGame();
            Player.Instance.DestroyAllCaps();
            Bot.Instance.DestroyAllCaps();
            IsPlayerTurn = true;
            return;
        }

        IsPlayerTurn = !IsPlayerTurn;

        StartTurn();
    }

    IEnumerator BotTurn()
    {
        // ��������� ������� �� �����������
        yield return new WaitForSeconds(CapSpawner.Instance.CooldownDuration + 0.5f);

        Debug.Log("Bot Turn");

        CapMover.Instance.MoveCaps();
    }
}
