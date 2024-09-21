using UnityEngine;

public class PogController : MonoBehaviour
{
    private RandomMovement randomMovement;


    private static float initialSpeed = 5.0f;
    private float currentSpeed = initialSpeed;
    private float acceleration = 5.0f;
    private float maxSpeed = 15.0f;

    // Точка соприкосновения фишек со столом
    public const float HEIGHT_OF_CONTACT = 0.1f;

    void Start()
    {
        randomMovement = gameObject.AddComponent<RandomMovement>();

        if (randomMovement != null)
        {
            randomMovement.ApplyRandomMove();
        }
        else
        {
            Debug.LogError("Компонент RandomMovement не найден!");
        }
    }

    void Update()
    {
        // При пересечении точки соприкосновения + небольшое число т.к там задержка
        if (transform.position.y <= HEIGHT_OF_CONTACT)
        {
            // Сброс скорости к стандартной
            currentSpeed = initialSpeed;
            return;
        }

        // Ускорение и отрисовка этого ускорения
        currentSpeed += acceleration * Time.deltaTime;

        currentSpeed = Mathf.Clamp(currentSpeed, 0.0f, maxSpeed);

        transform.Translate(Vector3.down * currentSpeed * Time.deltaTime);
    }
}
