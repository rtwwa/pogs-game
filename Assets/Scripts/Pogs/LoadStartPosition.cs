using UnityEngine;


public class LoadStartPosition : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;

    [SerializeField]
    private float cooldownDuration = 1.0f;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    [SerializeField]
    bool randomMovementOnReload;

    private RandomMovement randomMovement;

    void Start()
    {
        startPosition = transform.position;

        if (!randomMovementOnReload)
            startRotation = transform.rotation;

        if (randomMovementOnReload && randomMovement == null)
        {
            randomMovement = GetComponent<RandomMovement>();
            if (randomMovement == null)
            {
                Debug.LogError("Объект randomMovement не найден на этом объекте!");
            }
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
            ResetOrMove();
        }
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
            ResetOrMove();
        }
    }

    void ResetOrMove()
    {
        Debug.Log($"Object Name: {gameObject.name}");
        GameObject currentObject = this.gameObject;
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





    void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;
    }
}