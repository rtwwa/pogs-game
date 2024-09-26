using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CapMover : MonoBehaviour
{
    public static CapMover Instance { get; private set; }

    public event System.Action OnMoveCompleted;

    public Button DropButton;

    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

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
            MoveCaps();
        }
    }

    public bool MoveCaps()
    {
        DropButton.interactable = false;
        if (isOnCooldown) return false;

        TransformToStartPosition();
        OnMoveCompleted?.Invoke();
        return true;
    }

    void TransformToStartPosition()
    {
        for (int i = 0; i < CapSpawner.Instance.SpawnedCaps.Count; i++)
        {
            GameObject pog = CapSpawner.Instance.SpawnedCaps[i];
            pog.transform.position = pog.GetComponent<Cap>().GetStartPosition();

            if (i == CapSpawner.Instance.SpawnedCaps.Count - 1)
            {
                ExplosionTrigger explosionTrigger = pog.GetComponent<ExplosionTrigger>();
                if (explosionTrigger == null && pog.activeSelf)
                {
                    explosionTrigger = pog.AddComponent<ExplosionTrigger>();
                    explosionTrigger.SpawnedCaps = CapSpawner.Instance.SpawnedCaps;
                }
            }
        }
        StartCooldown();

        StartCoroutine(EnableButtonAfterCooldown(CapSpawner.Instance.CooldownDuration));
    }

    IEnumerator EnableButtonAfterCooldown(float waitTime)
    {
        // Ждем заданное количество секунд
        yield return new WaitForSeconds(waitTime);

        // Делаем кнопку снова активной
        DropButton.interactable = true;
    }

    public void OnButtonClick()
    {
        MoveCaps();
    }


    void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = CapSpawner.Instance.CooldownDuration;
    }
}
