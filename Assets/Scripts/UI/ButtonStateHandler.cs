using UnityEngine;
using UnityEngine.UI;

public class ButtonStateHandler : MonoBehaviour
{
    public Button button; // ������, � ������� ����� ����������� ������
    public Image buttonImage; // ����������� �� ������

    // ���� ��� ����������� ���������
    public Color disabledColor = new(0.5f, 0.5f, 0.5f, 1f); // ����� ����
    private Color originalColor;

    void Start()
    {
        // �������� �������� ���� ������
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }

        // ������������� �� ��������� ��������� ���������� ������
        button.onClick.AddListener(ToggleButtonState);
    }

    // �����, ������� �������� ���� ������ � ����������� �� � ���������
    public void ToggleButtonState()
    {
        if (button.interactable)
        {
            // ������ �������, ���������� �������� ����
            buttonImage.color = originalColor;
        }
        else
        {
            // ������ ���������, ������ ����������� �����
            buttonImage.color = disabledColor;
        }
    }

    // ��� ������������ ����� ������ ��������� ������ ����� ���� �����
    public void SetButtonInteractable(bool isActive)
    {
        button.interactable = isActive;
        ToggleButtonState(); // ��������� ����� ����
    }
}
