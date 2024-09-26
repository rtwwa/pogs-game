using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;    // Исходный размер кнопки
    public Vector3 hoverScale = new(1.2f, 1.2f, 1.2f); // Размер кнопки при наведении
    public float animationSpeed = 0.2f; // Скорость анимации

    private bool isHovered = false;

    void Start()
    {
        // Сохраняем исходный размер кнопки
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Если курсор наведён, увеличиваем размер
        if (isHovered)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, hoverScale, Time.deltaTime / animationSpeed);
        }
        else
        {
            // Возвращаем размер к оригинальному при отсутствии наведения
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime / animationSpeed);
        }
    }

    // Обрабатываем событие наведения мыши
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    // Обрабатываем событие выхода курсора с кнопки
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}

