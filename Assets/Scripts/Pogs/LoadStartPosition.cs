using UnityEditor.Rendering.LookDev;
using UnityEngine;


public class LoadStartPosition : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;

    [SerializeField]
    private float cooldownDuration = 2.0f;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    [SerializeField]
    bool randomMovementOnReload;

    [SerializeField]
    private RandomMovement randomMovement;

    private CameraShake cameraShake;

    void Start()
    {
        startPosition = transform.position;

        if (!randomMovementOnReload)
            startRotation = transform.rotation;

        if (randomMovementOnReload && randomMovement == null)
        {
            Debug.LogError("Объект randomMovement не присвоен!");
        }

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            cameraShake = mainCamera.GetComponent<CameraShake>();
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
            if (cameraShake != null)
            {
                cameraShake.StartShake();
            }

            if (!randomMovementOnReload)
            {
                transform.position = startPosition;
                transform.rotation = startRotation;
            }
            else
            {
                randomMovement.ApplyRandomRotation();
                randomMovement.ApplyRandomMove();
            }

            StartCooldown();
        }
    }

    void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;
    }
}