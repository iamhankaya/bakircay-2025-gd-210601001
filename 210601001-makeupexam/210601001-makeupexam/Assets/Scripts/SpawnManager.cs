using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnablePrefab
{
    public GameObject prefab; // Spawn edilecek prefab
    public int count; // Üretilecek nesne sayýsý
}

public class SpawnManager : MonoBehaviour
{
    public List<SpawnablePrefab> prefabsToSpawn; // Spawn edilecek prefablar ve sayýlarý
    public Vector2 spawnAreaMin; // Spawn alanýnýn sol alt köþesi (X, Z)
    public Vector2 spawnAreaMax; // Spawn alanýnýn sað üst köþesi (X, Z)
    public float spawnHeight = 2f; // Spawn yüksekliði
    public float minDistance = 1f; // Nesneler arasýndaki minimum mesafe

    public List<Vector3> spawnedPositions = new List<Vector3>(); // Spawn edilen pozisyonlar
    private List<GameObject> spawnedObjects = new List<GameObject>(); // Spawn edilen nesnelerin referanslarý

    void Start()
    {
        // Baþlangýçta nesneleri spawn et
        SpawnAllObjects();
    }

    public void SpawnAllObjects()
    {
        foreach (var spawnable in prefabsToSpawn)
        {
            SpawnObjectsForPrefab(spawnable.prefab, spawnable.count);
        }
    }

    void SpawnObjectsForPrefab(GameObject prefab, int count)
    {
        int spawned = 0;
        int maxAttempts = 100; // Sonsuz döngüyü önlemek için bir sýnýr belirliyoruz

        while (spawned < count)
        {
            if (TrySpawnPrefab(prefab))
            {
                spawned++;
            }
            else
            {
                maxAttempts--;
                if (maxAttempts <= 0)
                {
                    Debug.LogWarning($"Maksimum deneme sayýsýna ulaþýldý. {prefab.name} için daha fazla nesne spawn edilemiyor.");
                    break;
                }
            }
        }
    }

    bool TrySpawnPrefab(GameObject prefab)
    {
        // Rastgele bir X-Z konumu seç
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            spawnHeight, // Yükseklik
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        // Pozisyon uygun mu kontrol et
        if (IsPositionValid(randomPosition))
        {
            // Nesneyi spawn et ve referansýný kaydet
            GameObject spawnedObject = Instantiate(prefab, randomPosition, Quaternion.identity);
            spawnedPositions.Add(randomPosition);
            spawnedObjects.Add(spawnedObject);
            return true;
        }

        return false;
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            // Daha önceki pozisyonlarla mesafeyi kontrol et
            if (Vector3.Distance(position, spawnedPosition) < minDistance)
            {
                return false; // Pozisyon çok yakýn, geçersiz
            }
        }

        return true; // Pozisyon uygun
    }

    // Oyun sýfýrlama fonksiyonu
    public void ResetSpawnedObjects()
    {
        // Spawn edilmiþ nesneleri sil
        foreach (var obj in spawnedObjects)
        {
            Destroy(obj); // Nesneyi sahneden kaldýr
        }
        spawnedObjects.Clear(); // Nesnelerin referanslarýný sýfýrla

        spawnedPositions.Clear(); // Spawn edilen pozisyonlarý sýfýrla
    }
}
