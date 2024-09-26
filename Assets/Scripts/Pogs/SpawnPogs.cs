//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class SpawnPogs : MonoBehaviour
//{
//    public static SpawnPogs Instance { get; private set; }

//    void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject); // Если нужно сохранить объект при загрузке новой сцены
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    [SerializeField] private int numberOfModels = 8;
//    [SerializeField] private Vector3 startPosition = new(0, 1.5f, 0);
//    [SerializeField] private float heightIncrement = 0.04f;

//    [SerializeField]
//    public float CooldownDuration { get; } = 2.5f;
//    private bool isOnCooldown = false;
//    private float cooldownTimer = 0f;

//    public Button myButton;

//    // Функция вызываемая после MakeMove()
//    public event System.Action OnMoveCompleted;


//    public List<GameObject> SpawnedCaps { get; set; } = new List<GameObject>();

//    void Start()
//    {
//        Spawn();
//    }

//    void Update()
//    {
//        if (isOnCooldown)
//        {
//            cooldownTimer -= Time.deltaTime;

//            if (cooldownTimer <= 0f)
//            {
//                isOnCooldown = false;
//            }
//        }

//        if (Input.GetKeyDown(KeyCode.Space) && !isOnCooldown)
//        {
//            MakeMove();
//        }
//    }

//    public bool MakeMove()
//    {
//        myButton.interactable = false;

//        if (isOnCooldown)
//        {
//            cooldownTimer -= Time.deltaTime;

//            if (cooldownTimer <= 0f)
//            {
//                isOnCooldown = false;
//            }

//            return false;
//        }

//        if (!isOnCooldown)
//        {
//            TransformToStartPosition();
//        }

//        OnMoveCompleted?.Invoke();


//        return true;
//    }
    
//    void TransformToStartPosition()
//    {
//        for (int i = 0; i < SpawnedCaps.Count; i++)
//        {
//            GameObject pog = SpawnedCaps[i];

//            // Установка позиции на стартовую
//            pog.transform.position = pog.GetComponent<Cap>().GetStartPosition();

//            // Если это последний элемент списка
//            if (i == SpawnedCaps.Count - 1)
//            {
//                // Попытка найти существующий компонент ExplosionTrigger
//                ExplosionTrigger explosionTrigger = pog.GetComponent<ExplosionTrigger>();

//                // Проверка, существует ли компонент и активен ли объект
//                if (explosionTrigger == null && pog.activeSelf)
//                {
//                    // Если компонента нет и объект активен, добавляем новый
//                    explosionTrigger = pog.AddComponent<ExplosionTrigger>();
//                    explosionTrigger.SpawnedCaps = SpawnedCaps;
//                }
//                else
//                {
//                    bool foundActive = false;

//                    // Проверка предыдущих объектов в списке
//                    for (int j = SpawnedCaps.Count - 1; j >= 0; j--)
//                    {
//                        if (SpawnedCaps[j].activeSelf)
//                        {
//                            explosionTrigger = SpawnedCaps[j].GetComponent<ExplosionTrigger>();

//                            // Проверка, существует ли компонент и активен ли объект
//                            if (explosionTrigger == null && SpawnedCaps[j].activeSelf)
//                            {
//                                explosionTrigger = SpawnedCaps[j].AddComponent<ExplosionTrigger>();
//                                explosionTrigger.SpawnedCaps = SpawnedCaps;
//                                foundActive = true;
//                                break;
//                            }

//                            foundActive = true;
//                            break;
//                        }
//                    }

//                    // Если не нашли активных объектов, можно дополнительно обработать это
//                    if (!foundActive)
//                    {
//                        if (!foundActive)
//                        {
//                            StartCoroutine(DestroyAndRespawn());
//                        }
//                    }
//                }
//            }


//            StartCooldown();

//            StartCoroutine(EnableButtonAfterCooldown(CooldownDuration));
//        }
//    }

//    IEnumerator DestroyAndRespawn()
//    {
//        // Удаляем все объекты в списке
//        for (int j = SpawnedCaps.Count - 1; j >= 0; j--)
//        {
//            if (SpawnedCaps[j] != null)
//            {
//                Destroy(SpawnedCaps[j]);  // Удаление объекта
//            }
//        }

//        // Очищаем список после удаления
//        SpawnedCaps.Clear();

//        // Ждем до конца кадра, чтобы все объекты были корректно уничтожены
//        yield return new WaitForEndOfFrame();

//        // Вызов метода Spawn для создания новых объектов
//        Spawn();
//    }

//    void Spawn()
//    {
//        myButton.interactable = false;

//        for (int i = 0; i < numberOfModels; i++)
//        {
//            Vector3 position = startPosition + new Vector3(0, heightIncrement * i, 0);
//            // Случайный градус вращения
//            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

//            // Спавн фишки с именем
//            GameObject spawnedModel = Cap.SpawnCap("Bulbasaur", position, rotation);

//            spawnedModel.AddComponent<PogController>();

//            SpawnedCaps.Add(spawnedModel);
//        }

//        StartCooldown();
//        StartCoroutine(EnableButtonAfterCooldown(CooldownDuration));
//    }

//    public void OnButtonClick()
//    {
//        MakeMove();
//    }

//    IEnumerator EnableButtonAfterCooldown(float waitTime)
//    {
//        // Ждем заданное количество секунд
//        yield return new WaitForSeconds(waitTime);

//        // Делаем кнопку снова активной
//        myButton.interactable = true;
//    }

//    void StartCooldown()
//    {
//        isOnCooldown = true;
//        cooldownTimer = CooldownDuration;
//    }
//}
