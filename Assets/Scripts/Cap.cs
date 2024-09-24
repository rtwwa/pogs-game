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

        // Создаём новый объект фишки
        GameObject newCap = Instantiate(modelPrefab, position, rotation);

        Cap cap = newCap.AddComponent<Cap>();

        cap.SetCapName(newCapName);
        newCap.name = newCapName;

        return newCap;
    }


    void Start()
    {
        startPosition = transform.position;

        // Загружаем модель по пути
        modelPrefab = Resources.Load<GameObject>(modelPrefabPath);

        // Находим компонент MeshRenderer у объекта фишки
        meshRenderer = GetComponent<MeshRenderer>();

        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

        meshRenderer.SetPropertyBlock(materialPropertyBlock);

        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer не найден на объекте фишки!");
            return;
        }

        texturePath = "Pogs/Materials/" + capName;

        // Пробуем установить текстуру и материал при инициализации
        bool success = SetTextureAndMaterial(texturePath);
        if (!success)
        {
            Debug.LogError("Ошибка при загрузке текстуры или материала.");
        }
    }

    bool SetTextureAndMaterial(string texturePath)
    {
        // Загружаем текстуру и материал по динамически созданному пути
        Material textureMaterial = Resources.Load<Material>(texturePath);

        // Проверяем, были ли успешно загружены ресурсы
        if (textureMaterial == null)
        {
            Debug.LogError("Текстура не найдена по пути: " + texturePath);
            return false;
        }

        // Применяем материалы к объекту
        meshRenderer.material = textureMaterial;

        return true;
    }


    public void SetCapName(string newName) { capName = newName; }
    public Vector3 GetStartPosition() { return startPosition; }
    public void Flipped() { isFlipped = !isFlipped; }
    public bool IsFlipped() { return isFlipped; }
}
