using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnablePrefab
{
    public GameObject prefab; // Spawn edilecek prefab
    public int count; // �retilecek nesne say�s�
}

public class SpawnManager : MonoBehaviour
{
    public List<SpawnablePrefab> prefabsToSpawn; // Spawn edilecek prefablar ve say�lar�
    public Vector2 spawnAreaMin; // Spawn alan�n�n sol alt k��esi (X, Z)
    public Vector2 spawnAreaMax; // Spawn alan�n�n sa� �st k��esi (X, Z)
    public float spawnHeight = 2f; // Spawn y�ksekli�i
    public float minDistance = 1f; // Nesneler aras�ndaki minimum mesafe

    public List<Vector3> spawnedPositions = new List<Vector3>(); // Spawn edilen pozisyonlar
    private List<GameObject> spawnedObjects = new List<GameObject>(); // Spawn edilen nesnelerin referanslar�

    void Start()
    {
        // Ba�lang��ta nesneleri spawn et
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
        int maxAttempts = 100; // Sonsuz d�ng�y� �nlemek i�in bir s�n�r belirliyoruz

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
                    Debug.LogWarning($"Maksimum deneme say�s�na ula��ld�. {prefab.name} i�in daha fazla nesne spawn edilemiyor.");
                    break;
                }
            }
        }
    }

    bool TrySpawnPrefab(GameObject prefab)
    {
        // Rastgele bir X-Z konumu se�
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            spawnHeight, // Y�kseklik
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        // Pozisyon uygun mu kontrol et
        if (IsPositionValid(randomPosition))
        {
            // Nesneyi spawn et ve referans�n� kaydet
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
            // Daha �nceki pozisyonlarla mesafeyi kontrol et
            if (Vector3.Distance(position, spawnedPosition) < minDistance)
            {
                return false; // Pozisyon �ok yak�n, ge�ersiz
            }
        }

        return true; // Pozisyon uygun
    }

    // Oyun s�f�rlama fonksiyonu
    public void ResetSpawnedObjects()
    {
        // Spawn edilmi� nesneleri sil
        foreach (var obj in spawnedObjects)
        {
            Destroy(obj); // Nesneyi sahneden kald�r
        }
        spawnedObjects.Clear(); // Nesnelerin referanslar�n� s�f�rla

        spawnedPositions.Clear(); // Spawn edilen pozisyonlar� s�f�rla
    }
}
