using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    private readonly string prefabPath = "VFX/Explosion";
    private GameObject explosionPrefab;
    private bool explosionTriggered = false;

    public List<GameObject> SpawnedCaps { get; set; }

    [SerializeField] private float maxWidth = 0.7f;
    [SerializeField] private float maxHeight = 0.5f;
    [SerializeField] private float maxLength = 0.7f;

    private readonly float circleRadius = Cap.CAP_DIAMETER;

    [SerializeField] private float minDistanceBetweenObjects;

    [SerializeField] private float moveDuration = 0.7f;

    private AudioSource audioSource;
    public AudioClip soundAllDrop;
    public AudioClip soundSoloDrop;

    void Start()
    {
        explosionPrefab = Resources.Load<GameObject>(prefabPath);

        audioSource = GetComponent<AudioSource>();

        audioSource = gameObject.AddComponent<AudioSource>();

        soundAllDrop = Resources.Load<AudioClip>("Sounds/AllDrop");
        soundSoloDrop = Resources.Load<AudioClip>("Sounds/SoloDrop");

        if (soundAllDrop == null)
        {
            Debug.LogError("soundAllDrop �� ������ � ����� Resources/Sounds.");
        }

        if (soundSoloDrop == null)
        {
            Debug.LogError("soundSoloDrop �� ������ � ����� Resources/Sounds.");
        }


        if (explosionPrefab == null)
        {
            Debug.LogError("Prefab ������ �� ��������.");
        }
    }

    void Update()
    {
        if (explosionPrefab != null && transform.position.y < 0.1f && !explosionTriggered)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            PlaySoundAllDrop();

            ScatterObjects();

            explosionTriggered = true;
        }
        if (transform.position.y > 1f)
        {
            explosionTriggered = false;
        }
    }

    public void PlaySoundAllDrop()
    {
        if (audioSource != null && soundAllDrop != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);

            audioSource.volume = Random.Range(0.8f, 1.0f);

            audioSource.clip = soundAllDrop;
            audioSource.Play();
        }
    }

    public void PlaySoundSoloDrop()
    {
        if (audioSource != null && soundSoloDrop != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.volume = Random.Range(0.8f, 1.0f);

            audioSource.clip = soundSoloDrop;
            audioSource.Play();
        }
    }

    void ScatterObjects()
    {
        List<Vector3> newPositions = new();

        foreach (GameObject pog in SpawnedCaps)
        {
            Vector3 newPosition = GetRandomPosition();

            while (!IsPositionFree(newPosition, newPositions))
            {
                newPosition = GetRandomPosition();
            }

            newPositions.Add(newPosition);
        }

        if (!Session.Instance.IsPlayerTurn)
        {
            for (int i = 0; i < SpawnedCaps.Count; i++)
            {
                StartCoroutine(MoveToPosition(SpawnedCaps[i], newPositions[i], Player.Instance.MoveReceivedCaps));
            }
        }
        else
        {
            for (int i = 0; i < SpawnedCaps.Count; i++)
            {
                StartCoroutine(MoveToPosition(SpawnedCaps[i], newPositions[i], Bot.Instance.MoveReceivedCaps));
            }
        }
    }

    IEnumerator MoveToPosition(GameObject pog, Vector3 targetPosition, System.Action<GameObject> onComplete)
    {
        System.Action<GameObject> OnComplete = onComplete;

        Vector3 startPosition = pog.transform.position;
        float elapsedTime = 0f;

        Vector3 controlPoint = (startPosition + targetPosition) / 2;
        controlPoint.y += maxHeight + Random.Range(-(maxHeight / 2), maxHeight / 2);

        bool shouldFlip = Random.value > 0.5f;

        if (shouldFlip)
        {
            Destroy(pog.GetComponent<PogController>());
            pog.GetComponent<Cap>().Flipped();
        }

        Quaternion startRotation = pog.transform.rotation;
        Quaternion endRotation = startRotation;

        if (shouldFlip)
        {
            endRotation *= Quaternion.Euler(0, 180f, 180);
        }

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;

            Vector3 newPosition = Mathf.Pow(1 - t, 2) * startPosition +
                                  2 * (1 - t) * t * controlPoint +
                                  Mathf.Pow(t, 2) * targetPosition;

            pog.transform.position = newPosition;

            if (shouldFlip)
            {
                pog.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pog.transform.SetPositionAndRotation(targetPosition, shouldFlip ? endRotation : startRotation);

        PlaySoundSoloDrop();

        yield return new WaitForSeconds(1f);

        if (shouldFlip)
        {
            SpawnedCaps.Remove(pog);
            OnComplete?.Invoke(pog);
        }    
    }

    Vector3 GetRandomPosition()
    {
        Vector2 randomPositionInCircle = GetRandomPointInCircle(circleRadius);

        float scaledX = Mathf.Lerp(-maxWidth / 2, maxWidth / 2, (randomPositionInCircle.x + circleRadius) / (2 * circleRadius));
        float scaledZ = Mathf.Lerp(-maxLength / 2, maxLength / 2, (randomPositionInCircle.y + circleRadius) / (2 * circleRadius));

        return new Vector3(scaledX, 0.1f, scaledZ);
    }

    Vector2 GetRandomPointInCircle(float circleRadius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float radius = Mathf.Sqrt(Random.Range(0f, 1f)) * circleRadius;
        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);
        return new Vector2(x, y);
    }

    bool IsPositionFree(Vector3 position, List<Vector3> existingPositions)
    {
        foreach (Vector3 pos in existingPositions)
        {
            if (Vector2.Distance(new Vector2(pos.x, pos.z), new Vector2(position.x, position.z)) < minDistanceBetweenObjects)
            {
                return false;
            }
        }

        foreach (GameObject pog in SpawnedCaps)
        {
            if (Vector2.Distance(new Vector2(pog.transform.position.x, pog.transform.position.z), new Vector2(position.x, position.z)) < minDistanceBetweenObjects)
            {
                return false;
            }
        }

        return true;
    }

    //// ��������� � ������ �����???
    //IEnumerator FadeOut(GameObject pog)
    //{
    //    Renderer renderer = pog.GetComponent<Renderer>();
    //    Material[] materials = renderer.materials;  // �������� ������ ���� ����������
    //    float fadeDuration = 1.0f;  // ������������ ������������
    //    float elapsedTime = 0f;

    //    // ��������� ������������ ����� ���� ����������
    //    List<Color> originalColors = new List<Color>();
    //    foreach (Material mat in materials)
    //    {
    //        originalColors.Add(mat.color);
    //    }

    //    // �������� ������������
    //    while (elapsedTime < fadeDuration)
    //    {
    //        float t = elapsedTime / fadeDuration;

    //        for (int i = 0; i < materials.Length; i++)
    //        {
    //            Color newColor = originalColors[i];
    //            newColor.a = Mathf.Lerp(1, 0, t);  // ��������� �����-����� ��� ������������
    //            materials[i].color = newColor;
    //        }

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    // ��������� ������� ������ ����� ������������
    //    for (int i = 0; i < materials.Length; i++)
    //    {
    //        Color finalColor = materials[i].color;
    //        finalColor.a = 0;
    //        materials[i].color = finalColor;
    //    }

    //    // ������������ ������
    //    pog.SetActive(false);
    //}
}
