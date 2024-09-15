using UnityEngine;

public class PogController : MonoBehaviour
{
    [SerializeField]
    private RandomMovement randomMovement;

    [SerializeField]
    private ApplyForce applyForceScript;

    private CameraShake cameraShake;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            cameraShake = mainCamera.GetComponent<CameraShake>();
        }

        if (randomMovement != null)
        {
            randomMovement.ApplyRandomMove();
            randomMovement.ApplyRandomRotation();

            if (cameraShake != null)
            {
                cameraShake.StartShake();
            }
        }
        else
        {
            Debug.LogError("Объект randomMovement не присвоен!");
        }

        if (applyForceScript == null)
        {
            Debug.LogError("Скрипт ApplyForce не присвоен!");
        }
    }

    void FixedUpdate()
    {
        if (applyForceScript != null)
        {
            applyForceScript.ApplyForceToRigidBody(rb);
        }
    }

}