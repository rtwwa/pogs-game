using UnityEngine;

public class Cap : MonoBehaviour
{
    private string capName = "Bulbasaur";

    private GameObject modelPrefab;
    private static string modelPrefabPath = "Models/Pog";

    private MeshRenderer meshRenderer;

    public const float CAP_DIAMETER = 0.4f;

    private string texturePath;

    private Vector3 startPosition;

    [SerializeField]
    private bool isFlipped = false;

    public static GameObject SpawnCap(string newCapName, Vector3 position, Quaternion rotation)
    {
        GameObject modelPrefab = Resources.Load<GameObject>(modelPrefabPath);

        if (modelPrefab == null)
        {
            Debug.Log("Pog model not found.");
        }

        // ������ ����� ������ �����
        GameObject newCap = Instantiate(modelPrefab, position, rotation);

        Cap cap = newCap.AddComponent<Cap>();

        cap.SetCapName(newCapName);
        newCap.name = newCapName;

        return newCap;
    }


    void Start()
    {
        startPosition = transform.position;

        // ��������� ������ �� ����
        modelPrefab = Resources.Load<GameObject>(modelPrefabPath);

        // ������� ��������� MeshRenderer � ������� �����
        meshRenderer = GetComponent<MeshRenderer>();

        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

        meshRenderer.SetPropertyBlock(materialPropertyBlock);

        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer �� ������ �� ������� �����!");
            return;
        }

        texturePath = "Pogs/Materials/" + capName;

        // ������� ���������� �������� � �������� ��� �������������
        bool success = SetTextureAndMaterial(texturePath);
        if (!success)
        {
            Debug.LogError("������ ��� �������� �������� ��� ���������.");
        }
    }

    bool SetTextureAndMaterial(string texturePath)
    {
        // ��������� �������� � �������� �� ����������� ���������� ����
        Material textureMaterial = Resources.Load<Material>(texturePath);

        // ���������, ���� �� ������� ��������� �������
        if (textureMaterial == null)
        {
            Debug.LogError("�������� �� ������� �� ����: " + texturePath);
            return false;
        }

        // ��������� ��������� � �������
        meshRenderer.material = textureMaterial;

        return true;
    }


    public void SetCapName(string newName) { capName = newName; }
    public Vector3 GetStartPosition() { return startPosition; }
    public void Flipped() { isFlipped = !isFlipped; }
    public bool IsFlipped() { return isFlipped; }
}
