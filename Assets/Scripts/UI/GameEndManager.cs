using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndManager : MonoBehaviour
{
    [SerializeField]
    public GameObject gameOverMenu; // ������ � ����
    public TextMeshProUGUI finalScoreText;

    public static GameEndManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // ��������: �������� �� ������ ������
            if (gameOverMenu != null)
            {
                Debug.Log("������ ���������, �������� ��.");
                gameOverMenu.SetActive(false); // �������� ������
            }
            else
            {
                Debug.LogError("gameOverMenu �� ��������� � ����������!");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndGame()
    {
        Debug.Log("���� ���������!");

        // ���������� ����
        if (gameOverMenu != null)
        {
            Debug.Log("���������� ������.");
            finalScoreText.text = "����� ������: " + Player.Instance.MyCaps.Count.ToString() + "\n����� ����: " + Bot.Instance.MyCaps.Count.ToString();
            gameOverMenu.SetActive(true); // ���������� ������
        }
        else
        {
            Debug.LogError("gameOverMenu �� ��������� � ����������!");
        }
    }
}
