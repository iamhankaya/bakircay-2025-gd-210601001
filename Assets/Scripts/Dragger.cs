using UnityEngine;

public class Dragger : MonoBehaviour
{
    private ParticleSystem _particleSystem; // Nesne �zerindeki Particle System
    private bool _isDragging; // S�r�kleme durumunu takip eder
    private Vector3 _originalPosition; // Nesnenin ba�lang�� pozisyonu

    [SerializeField]
    private float liftHeight = 5f; // Nesnenin kalkaca�� y�kseklik

    private void Start()
    {
        _originalPosition = transform.position;
        _particleSystem = GetComponent<ParticleSystem>();

        // Ba�lang��ta Particle System'i kapal� tut
        if (_particleSystem != null)
        {
            _particleSystem.Stop();
        }
    }

    private void OnMouseDrag()
    {
        // S�r�kleme ba�lad���nda Particle System'i �al��t�r
        if (_particleSystem != null && !_isDragging)
        {
            _particleSystem.Play();
            _isDragging = true;
        }

        // Fare pozisyonunu takip ederek nesneyi hareket ettir
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void OnMouseUp()
    {
        // S�r�kleme bitti�inde Particle System'i durdur ve durumlar� s�f�rla
        if (_particleSystem != null && _isDragging)
        {
            _particleSystem.Stop();
            _isDragging = false;
        }

        // PlacementArea kontrol�
        GameObject placementArea = GameObject.Find("PlacementArea");
        if (placementArea != null)
        {
            Collider placementCollider = placementArea.GetComponent<Collider>();
            if (placementCollider != null)
            {
                // Gerekli i�lemleri buraya ekleyebilirsiniz.
            }
        }
    }

    // TryGetOriginalPosition metodunu ekleyelim
    public bool TryGetOriginalPosition(out Vector3 originalPosition)
    {
        originalPosition = _originalPosition;
        return true;
    }
}
