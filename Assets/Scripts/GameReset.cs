using UnityEngine;

public class GameReset : MonoBehaviour
{
    [SerializeField]
    private SpawnManager _spawnManager; // SpawnManager referans�
    [SerializeField]
    private PlacementController _placementController; // PlacementController referans�

    public void OnClick()
    {
        ResetGame();
    }

    private void ResetGame()
    {
        // SpawnManager'dan spawn edilen nesneleri s�f�rla
        if (_spawnManager != null)
        {
            _spawnManager.ResetSpawnedObjects(); // Spawn edilen nesneleri sil
            _spawnManager.SpawnAllObjects();     // Nesneleri yeniden olu�tur
        }
        else
        {
            Debug.LogError("SpawnManager referans� bulunamad�!");
        }

        // PlacementController'� bul ve skoru s�f�rla
        if (_placementController != null)
        {
            _placementController.ResetPlacementArea(); // Placement alan�n� s�f�rla
            _placementController.UpdateScore(-_placementController.Score); // Skoru s�f�rla
        }
        else
        {
            Debug.LogError("PlacementController referans� bulunamad�!");
        }

        Debug.Log("Oyun s�f�rland�!");
    }
}
