using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    private string prefabPath = "Explosion2";
    private GameObject explosionPrefab;
    private bool explosionTriggered = false;

    private List<GameObject> spawnedModels;

    [SerializeField] private int numberOfObjects = 8;
    [SerializeField] private float circleRadius = 0.09f;
    [SerializeField] private float maxWidth = 1f;
    [SerializeField] private float maxHeight = 0.5f;
    [SerializeField] private float maxLength = 1f;
    [SerializeField] private float minDistanceBetweenObjects = 0.1f;
    [SerializeField] private float moveDuration = 0.7f;

    private AudioSource audioSource;
    public AudioClip soundAllDrop;
    public AudioClip soundSoloDrop;

    void Start()
    {
        explosionPrefab = Resources.Load<GameObject>(prefabPath);

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        soundAllDrop = Resources.Load<AudioClip>("Sounds/AllDrop");
        soundSoloDrop = Resources.Load<AudioClip>("Sounds/SoloDrop");

        if (soundAllDrop == null)
        {
            Debug.LogError("soundAllDrop не найден в папке Resources/Sounds.");
        }

        if (soundSoloDrop == null)
        {
            Debug.LogError("soundSoloDrop не найден в папке Resources/Sounds.");
        }


        if (explosionPrefab == null)
        {
            Debug.LogError("Prefab взрыва не назначен.");
        }
    }

    void Update()
    {
        if (explosionPrefab != null && transform.position.y < 0.1f && !explosionTriggered)
        {
            Vector3 vector3 = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
            Instantiate(explosionPrefab, transform.position, transform.rotation);

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
        foreach (GameObject pog in spawnedModels)
        {
            Vector3 newPosition = GetRandomPosition();

            while (!IsPositionFree(newPosition))
            {
                newPosition = GetRandomPosition();
            }

            StartCoroutine(MoveToPosition(pog, newPosition));
        }
    }

    IEnumerator MoveToPosition(GameObject pog, Vector3 targetPosition)
    {
        Vector3 startPosition = pog.transform.position;
        float elapsedTime = 0f;

        Vector3 controlPoint = (startPosition + targetPosition) / 2;
        controlPoint.y += maxHeight + Random.Range(-(maxHeight / 2), maxHeight / 2);

        bool shouldFlip = Random.value > 0.5f;

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

        pog.transform.position = targetPosition;
        pog.transform.rotation = shouldFlip ? endRotation : startRotation;

        PlaySoundSoloDrop();
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

    bool IsPositionFree(Vector3 position)
    {
        foreach (GameObject pog in spawnedModels)
        {
            if (Vector3.Distance(pog.transform.position, position) < minDistanceBetweenObjects)
            {
                return false;
            }
        }
        return true;
    }

    public void SetSpawnedModels(List<GameObject> models)
    {
        spawnedModels = models;
    }
}
