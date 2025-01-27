using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlacementController : MonoBehaviour
{
    [SerializeField]
    private int _maxObjectsInArea = 2; // Placement alanýnda maksimum nesne sayýsý

    [SerializeField]
    private ParticleSystem _matchParticles; // Eþleþme partikül efektleri

    [SerializeField]
    private Transform _resetPosition; // Baþarýsýz nesnelerin döneceði pozisyon

    [SerializeField]
    private TextMeshProUGUI _scoreText; // Skor göstergesi (UI)

    [SerializeField]
    private TextMeshProUGUI _gameOverText; // Oyun bitti yazýsýný gösterecek UI

    private int _score = 0; // Toplam skor
    private readonly List<GameObject> _objectsInPlacementArea = new(); // Placement alanýndaki nesneler
    private bool _isMatchInProgress = false; // Eþleþme iþlemi devam ediyor mu?

    private float _timer = 60f; // Timer baþlangýç süresi (saniye olarak)
    private bool _gameOver = false; // Oyun bitti mi?

    public int Score => _score; // Skoru dýþarýya eriþilebilir hale getirdik

    private void Start()
    {
        UpdateScore(0); // Baþlangýçta skoru sýfýrla ve göster
        _gameOverText.gameObject.SetActive(false); // Oyun bitti yazýsýný gizle
    }

    private void Update()
    {
        if (!_gameOver)
        {
            // Timer'ý her frame azalt
            _timer -= Time.deltaTime;

            // Timer 0.00 olduðunda oyun bitmiþ sayýlýr
            if (_timer <= 0f)
            {
                _timer = 0f; // Timer'ý sýfýrla
                GameOver(); // Oyun bitti fonksiyonunu çaðýr
            }
        }
    }

    private void GameOver()
    {
        
            // Oyun bitti durumu
            _gameOver = true;

            // Game Over text'ini aktif et
            if (_gameOverText != null)
            {
                _gameOverText.gameObject.SetActive(true); // Oyun bitti yazýsýný ekrana getir

                _gameOverText.text = "Oyun Bitti!"; // Yazýyý güncelle
            }
            else
            {
                Debug.Log("GameOverText referansý eksik!");
            }

            Debug.Log("Oyun Bitti!");
        

    }

    private void OnTriggerEnter(Collider other)
    {
        // Eðer nesne "Spawnable" etiketine sahipse
        if (other.CompareTag("Spawnable"))
        {
            // Eðer alan dolu deðilse ve eþleþme yapýlmadýysa
            if (_objectsInPlacementArea.Count < _maxObjectsInArea && !_isMatchInProgress && !_gameOver)
            {
                // Nesne daha önce eklenmiþse atla
                if (!_objectsInPlacementArea.Contains(other.gameObject))
                {
                    _objectsInPlacementArea.Add(other.gameObject);
                    Debug.Log($"{other.name} placement alanýna eklendi.");

                    // Nesnenin kinematik özelliklerini deðiþtir (hareket etmesini engelle)
                    var rigidbody = other.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        rigidbody.isKinematic = true;
                    }
                }

                // Placement alaný dolduysa eþleþmeyi kontrol et
                if (_objectsInPlacementArea.Count == _maxObjectsInArea)
                {
                    CheckMatch();
                }
            }
            else if (_isMatchInProgress)
            {
                Debug.Log("Eþleþme iþlemi devam ediyor, yeni nesneler eklenemez.");
            }
            else
            {
                Debug.Log($"Placement alaný dolu. {other.name} reddedildi.");
                // Alan doluysa nesneyi orijinal pozisyona geri gönder
                ResetObject(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Eðer nesne "Spawnable" etiketine sahipse
        if (other.CompareTag("Spawnable"))
        {
            // Nesne çýkarsa listeden çýkar
            _objectsInPlacementArea.Remove(other.gameObject);
            Debug.Log($"{other.name} placement alanýndan çýkarýldý.");
        }
    }

    private void CheckMatch()
    {
        // Eðer placement alanýnda iki nesne varsa, eþleþmeyi kontrol et
        if (_objectsInPlacementArea.Count == _maxObjectsInArea)
        {
            _isMatchInProgress = true; // Eþleþme iþlemi baþladý

            var firstObject = _objectsInPlacementArea[0];
            var secondObject = _objectsInPlacementArea[1];

            // Eðer nesnelerin isimleri eþleþiyorsa, baþarýlý eþleþme
            if (firstObject.name == secondObject.name)
            {
                Debug.Log("Eþleþme baþarýlý: " + firstObject.name);
                UpdateScore(50); // Skoru artýr

                // Eþleþme partiküllerini çalýþtýr
                if (_matchParticles != null)
                {
                    _matchParticles.transform.position = firstObject.transform.position;
                    _matchParticles.Play();
                }

                // Nesneleri yok et
                Destroy(firstObject);
                Destroy(secondObject);
            }
            else
            {
                Debug.Log("Eþleþme baþarýsýz!");
                UpdateScore(-25); // Skoru düþür

                // Nesneleri orijinal pozisyonlarýna döndür
                ResetObject(firstObject);
                ResetObject(secondObject);
            }

            // Alaný temizle
            _objectsInPlacementArea.Clear();

            // Eþleþme bitince yeni nesne eklemeye izin ver
            _isMatchInProgress = false;
        }
    }

    private void ResetObject(GameObject obj)
    {
        // Nesnenin orijinal pozisyona dönmesini saðla
        var dragger = obj.GetComponent<Dragger>();
        if (dragger != null && dragger.TryGetOriginalPosition(out var originalPosition))
        {
            obj.transform.position = originalPosition;
        }
        else if (_resetPosition != null)
        {
            obj.transform.position = _resetPosition.position;
        }

        // Nesnenin rigidbody'sini eski haline getir
        var rigidbody = obj.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = false;
        }
        Debug.Log($"{obj.name} orijinal pozisyonuna döndürüldü.");
    }

    public void ResetPlacementArea()
    {
        // Placement alanýndaki tüm nesneleri sýfýrla
        foreach (var obj in _objectsInPlacementArea)
        {
            ResetObject(obj);
        }

        // Listeyi temizle
        _objectsInPlacementArea.Clear();
        Debug.Log("Placement alaný sýfýrlandý.");
    }

    public void UpdateScore(int amount)
    {
        // Skoru güncelle
        _score += amount;
        if (_scoreText != null)
        {
            _scoreText.text = "Score: " + _score;
        }
        Debug.Log("Güncel skor: " + _score);
    }
}
