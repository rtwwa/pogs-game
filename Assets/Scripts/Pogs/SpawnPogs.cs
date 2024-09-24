using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPogs : MonoBehaviour
{
    public static SpawnPogs Instance { get; private set; }

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

    [SerializeField] private int numberOfModels = 8;
    [SerializeField] private Vector3 startPosition = new Vector3(0, 1.5f, 0);
    [SerializeField] private float heightIncrement = 0.04f;

    [SerializeField]
    public float cooldownDuration { get; } = 2.5f;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    // Функция вызываемая после MakeMove()
    public event System.Action OnMoveCompleted;

    public List<GameObject> spawnedModels { get; set; } = new List<GameObject>();
    public List<GameObject> flippedCaps { get; } = new List<GameObject>();

    void Start()
    {
        Spawn();
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isOnCooldown)
        {
            MakeMove();
        }
    }

    public bool MakeMove()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }

            return false;
        }

        if (!isOnCooldown)
        {
            TransformToStartPosition();
        }

        OnMoveCompleted?.Invoke();


        return true;
    }
    
    void TransformToStartPosition()
    {
        for (int i = 0; i < spawnedModels.Count; i++)
        {
            GameObject pog = spawnedModels[i];

            // Размещение фишек по высоте через определенный промежуток
            Vector3 position = startPosition + new Vector3(0, heightIncrement * i, 0);
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            // Установка позиции на стартовую
            pog.transform.position = pog.GetComponent<Cap>().GetStartPosition();

            // Если это последний элемент списка
            if (i == spawnedModels.Count - 1)
            {
                // Попытка найти существующий компонент ExplosionTrigger
                ExplosionTrigger explosionTrigger = pog.GetComponent<ExplosionTrigger>();

                // Проверка, существует ли компонент и активен ли объект
                if (explosionTrigger == null && pog.activeSelf)
                {
                    // Если компонента нет и объект активен, добавляем новый
                    explosionTrigger = pog.AddComponent<ExplosionTrigger>();
                    explosionTrigger.SetSpawnedModels(spawnedModels);
                }
                else
                {
                    bool foundActive = false;

                    // Проверка предыдущих объектов в списке
                    for (int j = spawnedModels.Count - 1; j >= 0; j--)
                    {
                        if (spawnedModels[j].activeSelf)
                        {
                            explosionTrigger = spawnedModels[j].GetComponent<ExplosionTrigger>();

                            // Проверка, существует ли компонент и активен ли объект
                            if (explosionTrigger == null && spawnedModels[j].activeSelf)
                            {
                                explosionTrigger = spawnedModels[j].AddComponent<ExplosionTrigger>();
                                explosionTrigger.SetSpawnedModels(spawnedModels);
                                foundActive = true;
                                break;
                            }

                            foundActive = true;
                            break;
                        }
                    }

                    // Если не нашли активных объектов, можно дополнительно обработать это
                    if (!foundActive)
                    {
                        if (!foundActive)
                        {
                            StartCoroutine(DestroyAndRespawn());
                        }
                    }
                }
            }

            StartCooldown();
        }
    }

    IEnumerator DestroyAndRespawn()
    {
        // Удаляем все объекты в списке
        for (int j = spawnedModels.Count - 1; j >= 0; j--)
        {
            if (spawnedModels[j] != null)
            {
                Destroy(spawnedModels[j]);  // Удаление объекта
            }
        }

        // Очищаем список после удаления
        spawnedModels.Clear();

        // Ждем до конца кадра, чтобы все объекты были корректно уничтожены
        yield return new WaitForEndOfFrame();

        // Вызов метода Spawn для создания новых объектов
        Spawn();
    }

    void Spawn()
    {
        for (int i = 0; i < numberOfModels; i++)
        {
            Vector3 position = startPosition + new Vector3(0, heightIncrement * i, 0);
            // Случайный градус вращения
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            // Спавн фишки с именем
            GameObject spawnedModel = Cap.SpawnCap("Bulbasaur", position, rotation);

            spawnedModel.AddComponent<PogController>();

            spawnedModels.Add(spawnedModel);
        }

        StartCooldown();
    }

    public void OnButtonClick()
    {
        MakeMove();
    }

    void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;
    }
}
