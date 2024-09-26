using UnityEngine;
using UnityEngine.UI;

public class ButtonStateHandler : MonoBehaviour
{
    public Button button; // Кнопка, к которой будет применяться эффект
    public Image buttonImage; // Изображение на кнопке

    // Цвет для неактивного состояния
    public Color disabledColor = new(0.5f, 0.5f, 0.5f, 1f); // Серый цвет
    private Color originalColor;

    void Start()
    {
        // Получаем исходный цвет кнопки
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }

        // Подписываемся на изменение состояния активности кнопки
        button.onClick.AddListener(ToggleButtonState);
    }

    // Метод, который изменяет цвет кнопки в зависимости от её состояния
    public void ToggleButtonState()
    {
        if (button.interactable)
        {
            // Кнопка активна, возвращаем исходный цвет
            buttonImage.color = originalColor;
        }
        else
        {
            // Кнопка неактивна, делаем изображение серым
            buttonImage.color = disabledColor;
        }
    }

    // Для тестирования можно менять состояние кнопки через этот метод
    public void SetButtonInteractable(bool isActive)
    {
        button.interactable = isActive;
        ToggleButtonState(); // Применяем новый цвет
    }
}
