using System.Collections.Generic;
using UnityEngine;

public class SpawnPogs : MonoBehaviour
{
    [SerializeField] private GameObject modelPrefab;
    [SerializeField] private int numberOfModels = 8;
    [SerializeField] private Vector3 startPosition = new Vector3(0, 1, 0);
    [SerializeField] private float heightIncrement = 0.04f;

    [SerializeField] private string texturePath = "Pogs/Bulbasaur";
    [SerializeField] private string materialPath = "Pogs/BulbasaurSides";

    private List<GameObject> spawnedModels = new List<GameObject>();

    private LoadStartPosition loadStartPosition;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        Texture2D texture = Resources.Load<Texture2D>(texturePath);

        Material predefinedMaterial = Resources.Load<Material>(materialPath);

        if (texturePath == null || predefinedMaterial == null)
        {
            Debug.LogError("“екстура или материал не найдены по пути: " + texturePath + " или " + materialPath);
            return;
        }

        for (int i = 0; i < numberOfModels; i++)
        {
            Vector3 position = startPosition + new Vector3(0, heightIncrement * i, 0);
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            GameObject spawnedModel = Instantiate(modelPrefab, position, rotation);

            spawnedModel.AddComponent<PogController>();
            loadStartPosition = spawnedModel.AddComponent<LoadStartPosition>();

            Renderer renderer = spawnedModel.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                material.mainTexture = texture;

                renderer.materials = new Material[] { predefinedMaterial, material };
            }
            else
            {
                Debug.LogError("ћодель не имеет Renderer, чтобы назначить материалы.");
            }

            spawnedModels.Add(spawnedModel);

            if (i == numberOfModels - 2)
            {
                ExplosionTrigger explosionTrigger = spawnedModel.AddComponent<ExplosionTrigger>();
                explosionTrigger.SetSpawnedModels(spawnedModels);
            }
        }
    }
}
