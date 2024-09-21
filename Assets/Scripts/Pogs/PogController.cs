using UnityEngine;

public class PogController : MonoBehaviour
{
    private RandomMovement randomMovement;
    private ApplyForce applyForceScript;
    private Rigidbody rb;
    private LoadStartPosition loadStartPosition;

    private float currentSpeed = 7.0f;
    private float acceleration = 5.0f;
    private float maxSpeed = 15.0f;

    void Start()
    {
        randomMovement = GetComponent<RandomMovement>();
        if (randomMovement == null)
        {
            randomMovement = gameObject.AddComponent<RandomMovement>();
            Debug.Log("RandomMovement �������� �������������.");
        }

        applyForceScript = GetComponent<ApplyForce>();
        if (applyForceScript == null)
        {
            applyForceScript = gameObject.AddComponent<ApplyForce>();
            Debug.Log("ApplyForce �������� �������������.");
        }

        loadStartPosition = GetComponent<LoadStartPosition>();
        if (loadStartPosition == null)
        {
            loadStartPosition = gameObject.AddComponent<LoadStartPosition>();
            Debug.Log("LoadStartPosition �������� �������������.");
        }

        if (randomMovement != null)
        {
            randomMovement.ApplyRandomMove();
        }
        else
        {
            Debug.LogError("��������� RandomMovement �� ������!");
        }

        if (applyForceScript == null)
        {
            Debug.LogError("��������� ApplyForce �� ������!");
        }
    }

    void Update()
    {
        if (transform.position.y <= 0.1f)
        {
            currentSpeed = 5.0f;
            return;
        }

        currentSpeed += acceleration * Time.deltaTime;

        currentSpeed = Mathf.Clamp(currentSpeed, 0.0f, maxSpeed);

        transform.Translate(Vector3.down * currentSpeed * Time.deltaTime);
    }
}
