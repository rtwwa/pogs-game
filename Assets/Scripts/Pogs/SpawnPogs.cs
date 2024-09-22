using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPogs : MonoBehaviour
{
    [SerializeField] private int numberOfModels = 8;
    [SerializeField] private Vector3 startPosition = new Vector3(0, 1.5f, 0);
    [SerializeField] private float heightIncrement = 0.04f;

    [SerializeField]
    private float cooldownDuration = 2.5f;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    private List<GameObject> spawnedModels = new List<GameObject>();

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
            TransformToStartPosition();
        }
    }

    void Start()
    {
        Spawn();
    }

    void TransformToStartPosition()
    {
        for (int i = 0; i < spawnedModels.Count; i++)
        {
            GameObject pog = spawnedModels[i];

            // ���������� ����� �� ������ ����� ������������ ����������
            Vector3 position = startPosition + new Vector3(0, heightIncrement * i, 0);
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            // ��������� ������� �� ���������
            pog.transform.position = pog.GetComponent<Cap>().GetStartPosition();

            // ���� ��� ��������� ������� ������
            if (i == spawnedModels.Count - 1)
            {
                // ������� ����� ������������ ��������� ExplosionTrigger
                ExplosionTrigger explosionTrigger = pog.GetComponent<ExplosionTrigger>();

                // ��������, ���������� �� ��������� � ������� �� ������
                if (explosionTrigger == null && pog.activeSelf)
                {
                    // ���� ���������� ��� � ������ �������, ��������� �����
                    explosionTrigger = pog.AddComponent<ExplosionTrigger>();
                    explosionTrigger.SetSpawnedModels(spawnedModels);
                }
                else
                {
                    bool foundActive = false;

                    // �������� ���������� �������� � ������
                    for (int j = spawnedModels.Count - 1; j >= 0; j--)
                    {
                        if (spawnedModels[j].activeSelf)
                        {
                            explosionTrigger = spawnedModels[j].GetComponent<ExplosionTrigger>();

                            // ��������, ���������� �� ��������� � ������� �� ������
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

                    // ���� �� ����� �������� ��������, ����� ������������� ���������� ���
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
        // ������� ��� ������� � ������
        for (int j = spawnedModels.Count - 1; j >= 0; j--)
        {
            if (spawnedModels[j] != null)
            {
                Destroy(spawnedModels[j]);  // �������� �������
            }
        }

        // ������� ������ ����� ��������
        spawnedModels.Clear();

        // ���� �� ����� �����, ����� ��� ������� ���� ��������� ����������
        yield return new WaitForEndOfFrame();

        // ����� ������ Spawn ��� �������� ����� ��������
        Spawn();
    }

    void Spawn()
    {
        for (int i = 0; i < numberOfModels; i++)
        {
            Vector3 position = startPosition + new Vector3(0, heightIncrement * i, 0);
            // ��������� ������ ��������
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            // ����� ����� � ������
            GameObject spawnedModel = Cap.SpawnCap("Bulbasaur", position, rotation);

            spawnedModel.AddComponent<PogController>();

            spawnedModels.Add(spawnedModel);

            if (i == numberOfModels - 1)
            {
                ExplosionTrigger explosionTrigger = spawnedModels[spawnedModels.Count - 1].AddComponent<ExplosionTrigger>();
                explosionTrigger.SetSpawnedModels(spawnedModels);
            }
        }

        StartCooldown();
    }

    public void OnButtonClick()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }

        if (!isOnCooldown)
        {
            Debug.Log("Drop");
            TransformToStartPosition();
        }
    }

    void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;
    }
}
