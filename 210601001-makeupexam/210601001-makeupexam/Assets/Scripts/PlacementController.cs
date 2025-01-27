using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlacementController : MonoBehaviour
{
    [SerializeField]
    private int _maxObjectsInArea = 2; // Placement alan�nda maksimum nesne say�s�

    [SerializeField]
    private ParticleSystem _matchParticles; // E�le�me partik�l efektleri

    [SerializeField]
    private Transform _resetPosition; // Ba�ar�s�z nesnelerin d�nece�i pozisyon

    [SerializeField]
    private TextMeshProUGUI _scoreText; // Skor g�stergesi (UI)

    [SerializeField]
    private TextMeshProUGUI _gameOverText; // Oyun bitti yaz�s�n� g�sterecek UI

    private int _score = 0; // Toplam skor
    private readonly List<GameObject> _objectsInPlacementArea = new(); // Placement alan�ndaki nesneler
    private bool _isMatchInProgress = false; // E�le�me i�lemi devam ediyor mu?

    private float _timer = 60f; // Timer ba�lang�� s�resi (saniye olarak)
    private bool _gameOver = false; // Oyun bitti mi?

    public int Score => _score; // Skoru d��ar�ya eri�ilebilir hale getirdik

    private void Start()
    {
        UpdateScore(0); // Ba�lang��ta skoru s�f�rla ve g�ster
        _gameOverText.gameObject.SetActive(false); // Oyun bitti yaz�s�n� gizle
    }

    private void Update()
    {
        if (!_gameOver)
        {
            // Timer'� her frame azalt
            _timer -= Time.deltaTime;

            // Timer 0.00 oldu�unda oyun bitmi� say�l�r
            if (_timer <= 0f)
            {
                _timer = 0f; // Timer'� s�f�rla
                GameOver(); // Oyun bitti fonksiyonunu �a��r
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
                _gameOverText.gameObject.SetActive(true); // Oyun bitti yaz�s�n� ekrana getir

                _gameOverText.text = "Oyun Bitti!"; // Yaz�y� g�ncelle
            }
            else
            {
                Debug.Log("GameOverText referans� eksik!");
            }

            Debug.Log("Oyun Bitti!");
        

    }

    private void OnTriggerEnter(Collider other)
    {
        // E�er nesne "Spawnable" etiketine sahipse
        if (other.CompareTag("Spawnable"))
        {
            // E�er alan dolu de�ilse ve e�le�me yap�lmad�ysa
            if (_objectsInPlacementArea.Count < _maxObjectsInArea && !_isMatchInProgress && !_gameOver)
            {
                // Nesne daha �nce eklenmi�se atla
                if (!_objectsInPlacementArea.Contains(other.gameObject))
                {
                    _objectsInPlacementArea.Add(other.gameObject);
                    Debug.Log($"{other.name} placement alan�na eklendi.");

                    // Nesnenin kinematik �zelliklerini de�i�tir (hareket etmesini engelle)
                    var rigidbody = other.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        rigidbody.isKinematic = true;
                    }
                }

                // Placement alan� dolduysa e�le�meyi kontrol et
                if (_objectsInPlacementArea.Count == _maxObjectsInArea)
                {
                    CheckMatch();
                }
            }
            else if (_isMatchInProgress)
            {
                Debug.Log("E�le�me i�lemi devam ediyor, yeni nesneler eklenemez.");
            }
            else
            {
                Debug.Log($"Placement alan� dolu. {other.name} reddedildi.");
                // Alan doluysa nesneyi orijinal pozisyona geri g�nder
                ResetObject(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // E�er nesne "Spawnable" etiketine sahipse
        if (other.CompareTag("Spawnable"))
        {
            // Nesne ��karsa listeden ��kar
            _objectsInPlacementArea.Remove(other.gameObject);
            Debug.Log($"{other.name} placement alan�ndan ��kar�ld�.");
        }
    }

    private void CheckMatch()
    {
        // E�er placement alan�nda iki nesne varsa, e�le�meyi kontrol et
        if (_objectsInPlacementArea.Count == _maxObjectsInArea)
        {
            _isMatchInProgress = true; // E�le�me i�lemi ba�lad�

            var firstObject = _objectsInPlacementArea[0];
            var secondObject = _objectsInPlacementArea[1];

            // E�er nesnelerin isimleri e�le�iyorsa, ba�ar�l� e�le�me
            if (firstObject.name == secondObject.name)
            {
                Debug.Log("E�le�me ba�ar�l�: " + firstObject.name);
                UpdateScore(50); // Skoru art�r

                // E�le�me partik�llerini �al��t�r
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
                Debug.Log("E�le�me ba�ar�s�z!");
                UpdateScore(-25); // Skoru d���r

                // Nesneleri orijinal pozisyonlar�na d�nd�r
                ResetObject(firstObject);
                ResetObject(secondObject);
            }

            // Alan� temizle
            _objectsInPlacementArea.Clear();

            // E�le�me bitince yeni nesne eklemeye izin ver
            _isMatchInProgress = false;
        }
    }

    private void ResetObject(GameObject obj)
    {
        // Nesnenin orijinal pozisyona d�nmesini sa�la
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
        Debug.Log($"{obj.name} orijinal pozisyonuna d�nd�r�ld�.");
    }

    public void ResetPlacementArea()
    {
        // Placement alan�ndaki t�m nesneleri s�f�rla
        foreach (var obj in _objectsInPlacementArea)
        {
            ResetObject(obj);
        }

        // Listeyi temizle
        _objectsInPlacementArea.Clear();
        Debug.Log("Placement alan� s�f�rland�.");
    }

    public void UpdateScore(int amount)
    {
        // Skoru g�ncelle
        _score += amount;
        if (_scoreText != null)
        {
            _scoreText.text = "Score: " + _score;
        }
        Debug.Log("G�ncel skor: " + _score);
    }
}
