using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public List<GameObject> MyCaps { get; set; } = new List<GameObject>();
    private Vector3 startPosition = new(-0.395f, 0.1f, -0.42f);
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Если нужно сохранить объект при загрузке новой сцены
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MoveReceivedCaps(GameObject cap)
    {
        MyCaps.Add(cap);

        Debug.Log("Object " + cap.name);

        for (int i = 0; i < MyCaps.Count; i++)
        {
            if (i == 0)
            {
                MyCaps[0].transform.SetPositionAndRotation(startPosition, Quaternion.Euler(0f, 0f, 180f));
            }
            else
            {
                MyCaps[i].transform.SetPositionAndRotation(startPosition + new Vector3(0, 0, -0.08f * i), Quaternion.Euler(-5f, 0f, 180f));
            }
        }
    }

    public void DestroyAllCaps()
    {
        // Проходим по всем объектам в списке и удаляем их
        for (int i = 0; i < MyCaps.Count; i++)
        {
            if (MyCaps[i] != null)
            {
                Destroy(MyCaps[i]);  // Удаляем объект из сцены
            }
        }

        // Очищаем список после удаления объектов
        MyCaps.Clear();
    }

}
