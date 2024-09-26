using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;    // �������� ������ ������
    public Vector3 hoverScale = new(1.2f, 1.2f, 1.2f); // ������ ������ ��� ���������
    public float animationSpeed = 0.2f; // �������� ��������

    private bool isHovered = false;

    void Start()
    {
        // ��������� �������� ������ ������
        originalScale = transform.localScale;
    }

    void Update()
    {
        // ���� ������ ������, ����������� ������
        if (isHovered)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, hoverScale, Time.deltaTime / animationSpeed);
        }
        else
        {
            // ���������� ������ � ������������� ��� ���������� ���������
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime / animationSpeed);
        }
    }

    // ������������ ������� ��������� ����
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    // ������������ ������� ������ ������� � ������
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}

