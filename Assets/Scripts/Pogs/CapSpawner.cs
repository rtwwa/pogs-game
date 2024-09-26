using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapSpawner : MonoBehaviour
{
    public static CapSpawner Instance { get; private set; }

    [SerializeField] private int numberOfModels = 8;
    [SerializeField] private Vector3 startPosition = new(0, 1.5f, 0);
    [SerializeField] private float heightIncrement = 0.04f;

    public Button RestartButton;
    public float CooldownDuration { get; } = 2.5f;

    public List<GameObject> SpawnedCaps { get; private set; } = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SpawnCaps();
    }

    public void SpawnCaps()
    {
        RestartButton.interactable = false;

        for (int i = 0; i < numberOfModels; i++)
        {
            Vector3 position = startPosition + new Vector3(0, heightIncrement * i, 0);
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            // Спавн фишки
            GameObject spawnedModel = Cap.SpawnCap("Bulbasaur", position, rotation);
            spawnedModel.AddComponent<PogController>();

            SpawnedCaps.Add(spawnedModel);
        }

        StartCoroutine(EnableButtonAfterCooldown(CooldownDuration));
    }
    public void OnButtonClick()
    {
        GameEndManager.Instance.gameOverMenu.SetActive(false);

        SpawnCaps();
    }


    IEnumerator EnableButtonAfterCooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        RestartButton.interactable = true;
    }
}
