using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Session : MonoBehaviour
{
    public static Session Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private Bot bot;

    [SerializeField] private SpawnPogs spawnPogs;

    public bool isPlayerTurn { get; set; } = true;  // ����������, ��� ������ ���

    private List<Cap> playerCaps;
    private List<Cap> botCaps;

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
        spawnPogs.OnMoveCompleted += EndTurn;

        // �������� ���� � ���� ������
        StartTurn();
    }

    void StartTurn()
    {
        if (isPlayerTurn)
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
        isPlayerTurn = !isPlayerTurn;
        StartTurn();
    }

    IEnumerator BotTurn()
    {
        // ��������� ������� �� �����������
        yield return new WaitForSeconds(spawnPogs.cooldownDuration + 0.5f);

        Debug.Log("Bot Turn");

        spawnPogs.MakeMove();
    }
}
