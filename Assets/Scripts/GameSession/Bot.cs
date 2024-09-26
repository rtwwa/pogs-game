using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public static Bot Instance { get; private set; }

    public List<GameObject> MyCaps { get; private set; } = new List<GameObject>();
    private Vector3 startPosition = new(0.345f, 0.1f, 0.42f);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void MoveReceivedCaps(GameObject cap)
    {
        MyCaps.Add(cap);

        for (int i = 0; i < MyCaps.Count; i++)
        {
            if (i == 0)
            {
                MyCaps[0].transform.SetPositionAndRotation(startPosition, Quaternion.Euler(0f, 180f, 180f));
            }
            else
            {
                MyCaps[i].transform.SetPositionAndRotation(startPosition + new Vector3(0, 0, 0.08f * i), Quaternion.Euler(-5f, 180f, 180f));
            }
        }
    }

    public void DestroyAllCaps()
    {
        // �������� �� ���� �������� � ������ � ������� ��
        for (int i = 0; i < MyCaps.Count; i++)
        {
            if (MyCaps[i] != null)
            {
                Destroy(MyCaps[i]);  // ������� ������ �� �����
            }
        }

        // ������� ������ ����� �������� ��������
        MyCaps.Clear();
    }

}
