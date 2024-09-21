using UnityEngine;

public class Cap : MonoBehaviour
{
    private string capName = "Bulbasaur";

    private GameObject modelPrefab;
    private static string modelPrefabPath = "Models/Pog";

    private MeshRenderer meshRenderer;

    public const float CAP_DIAMETER = 0.4f;

    private string texturePath;
    private string materialPath;

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

        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer не найден на объекте фишки!");
            return;
        }

        // Путь к текстурам на основе имени
        texturePath = "Pogs/Materials/" + capName;
        materialPath = "Pogs/" + capName + "Sides";

        // Пробуем установить текстуру и материал при инициализации
        bool success = SetTextureAndMaterial(texturePath, materialPath);
        if (!success)
        {
            Debug.LogError("Ошибка при загрузке текстуры или материала.");
        }
    }

    bool SetTextureAndMaterial(string texturePath, string materialPath)
    {
        // Загружаем текстуру и материал по динамически созданному пути
        Material textureMaterial = Resources.Load<Material>(texturePath);
        Material predefinedMaterial = Resources.Load<Material>(materialPath);

        // Проверяем, были ли успешно загружены ресурсы
        if (textureMaterial == null || predefinedMaterial == null)
        {
            Debug.LogError("Текстура или материал не найдены по пути: " + texturePath + " или " + materialPath);
            return false;
        }

        // Создаём массив для двух материалов
        Material[] materials = new Material[2];

        // Устанавливаем материалы в массив
        materials[0] = predefinedMaterial; // первый материал
        materials[1] = textureMaterial;    // второй материал

        // Применяем материалы к объекту
        meshRenderer.materials = materials;

        return true;
    }


    public void SetCapName(string newName) { capName = newName; }
    public Vector3 GetStartPosition() { return startPosition; }
    public void Flipped() { isFlipped = !isFlipped; }
    public bool IsFlipped() { return isFlipped; }
}
