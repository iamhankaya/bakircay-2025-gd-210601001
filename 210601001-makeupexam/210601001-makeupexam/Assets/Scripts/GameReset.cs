using UnityEngine;

public class GameReset : MonoBehaviour
{
    [SerializeField]
    private SpawnManager _spawnManager; // SpawnManager referansý
    [SerializeField]
    private PlacementController _placementController; // PlacementController referansý

    public void OnClick()
    {
        ResetGame();
    }

    private void ResetGame()
    {
        // SpawnManager'dan spawn edilen nesneleri sýfýrla
        if (_spawnManager != null)
        {
            _spawnManager.ResetSpawnedObjects(); // Spawn edilen nesneleri sil
            _spawnManager.SpawnAllObjects();     // Nesneleri yeniden oluþtur
        }
        else
        {
            Debug.LogError("SpawnManager referansý bulunamadý!");
        }

        // PlacementController'ý bul ve skoru sýfýrla
        if (_placementController != null)
        {
            _placementController.ResetPlacementArea(); // Placement alanýný sýfýrla
            _placementController.UpdateScore(-_placementController.Score); // Skoru sýfýrla
        }
        else
        {
            Debug.LogError("PlacementController referansý bulunamadý!");
        }

        Debug.Log("Oyun sýfýrlandý!");
    }
}
