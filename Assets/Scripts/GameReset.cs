using UnityEngine;

public class GameReset : MonoBehaviour
{
    [SerializeField]
    private SpawnManager _spawnManager; // SpawnManager referansı
    [SerializeField]
    private PlacementController _placementController; // PlacementController referansı
    [SerializeField]
    private Skill _gameSkill; // GameSkill referansı

    public void OnClick()
    {
        ResetGame();
    }

    private void ResetGame()
    {
        // SpawnManager'dan spawn edilen nesneleri sıfırla
        if (_spawnManager != null)
        {
            _spawnManager.ResetSpawnedObjects(); // Spawn edilen nesneleri sil
            _spawnManager.SpawnAllObjects();     // Nesneleri yeniden oluştur
        }
        else
        {
            Debug.LogError("SpawnManager referansı bulunamadı!");
        }

        // PlacementController'ı bul ve skoru sıfırla
        if (_placementController != null)
        {
            _placementController.ResetPlacementArea(); // Placement alanını sıfırla
            _placementController.UpdateScore(-_placementController.Score); // Skoru sıfırla
        }
        else
        {
            Debug.LogError("PlacementController referansı bulunamadı!");
        }

        // Timer'ı sıfırla
        if (_gameSkill != null)
        {
            _gameSkill.ResetTimer();
        }
        else
        {
            Debug.LogError("GameSkill referansı bulunamadı!");
        }

        Debug.Log("Oyun sıfırlandı!");
    }
}
