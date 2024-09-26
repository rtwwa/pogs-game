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
//            DontDestroyOnLoad(gameObject); // ���� ����� ��������� ������ ��� �������� ����� �����
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

//    // ������� ���������� ����� MakeMove()
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

//            // ��������� ������� �� ���������
//            pog.transform.position = pog.GetComponent<Cap>().GetStartPosition();

//            // ���� ��� ��������� ������� ������
//            if (i == SpawnedCaps.Count - 1)
//            {
//                // ������� ����� ������������ ��������� ExplosionTrigger
//                ExplosionTrigger explosionTrigger = pog.GetComponent<ExplosionTrigger>();

//                // ��������, ���������� �� ��������� � ������� �� ������
//                if (explosionTrigger == null && pog.activeSelf)
//                {
//                    // ���� ���������� ��� � ������ �������, ��������� �����
//                    explosionTrigger = pog.AddComponent<ExplosionTrigger>();
//                    explosionTrigger.SpawnedCaps = SpawnedCaps;
//                }
//                else
//                {
//                    bool foundActive = false;

//                    // �������� ���������� �������� � ������
//                    for (int j = SpawnedCaps.Count - 1; j >= 0; j--)
//                    {
//                        if (SpawnedCaps[j].activeSelf)
//                        {
//                            explosionTrigger = SpawnedCaps[j].GetComponent<ExplosionTrigger>();

//                            // ��������, ���������� �� ��������� � ������� �� ������
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

//                    // ���� �� ����� �������� ��������, ����� ������������� ���������� ���
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
//        // ������� ��� ������� � ������
//        for (int j = SpawnedCaps.Count - 1; j >= 0; j--)
//        {
//            if (SpawnedCaps[j] != null)
//            {
//                Destroy(SpawnedCaps[j]);  // �������� �������
//            }
//        }

//        // ������� ������ ����� ��������
//        SpawnedCaps.Clear();

//        // ���� �� ����� �����, ����� ��� ������� ���� ��������� ����������
//        yield return new WaitForEndOfFrame();

//        // ����� ������ Spawn ��� �������� ����� ��������
//        Spawn();
//    }

//    void Spawn()
//    {
//        myButton.interactable = false;

//        for (int i = 0; i < numberOfModels; i++)
//        {
//            Vector3 position = startPosition + new Vector3(0, heightIncrement * i, 0);
//            // ��������� ������ ��������
//            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

//            // ����� ����� � ������
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
//        // ���� �������� ���������� ������
//        yield return new WaitForSeconds(waitTime);

//        // ������ ������ ����� ��������
//        myButton.interactable = true;
//    }

//    void StartCooldown()
//    {
//        isOnCooldown = true;
//        cooldownTimer = CooldownDuration;
//    }
//}
