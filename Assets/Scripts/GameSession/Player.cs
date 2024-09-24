using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private List<GameObject> myCaps = new List<GameObject>();
    private Vector3 startPosition = new Vector3(-0.395f, 0.1f, -0.42f);
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
        myCaps.Add(cap);

        for (int i = 0; i < myCaps.Count - 1; i++)
        {
            if (i == 0)
            {
                myCaps[0].transform.position = startPosition;
            }
            else
            {
                myCaps[i].transform.position = startPosition + new Vector3(0, 0, -0.1f * i);
                myCaps[i].transform.rotation = Quaternion.Euler(-10f, 0, 0);
            }

            // Выводим текущие координаты объекта в консоль
            Debug.Log("Object " + i + " position: " + myCaps[i].transform.position);
        }
    }
}
