using UnityEngine;

public class Dragger : MonoBehaviour
{
    private ParticleSystem _particleSystem; // Nesne üzerindeki Particle System
    private bool _isDragging; // Sürükleme durumunu takip eder
    private Vector3 _originalPosition; // Nesnenin baþlangýç pozisyonu

    [SerializeField]
    private float liftHeight = 5f; // Nesnenin kalkacaðý yükseklik

    private void Start()
    {
        _originalPosition = transform.position;
        _particleSystem = GetComponent<ParticleSystem>();

        // Baþlangýçta Particle System'i kapalý tut
        if (_particleSystem != null)
        {
            _particleSystem.Stop();
        }
    }

    private void OnMouseDrag()
    {
        // Sürükleme baþladýðýnda Particle System'i çalýþtýr
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
        // Sürükleme bittiðinde Particle System'i durdur ve durumlarý sýfýrla
        if (_particleSystem != null && _isDragging)
        {
            _particleSystem.Stop();
            _isDragging = false;
        }

        // PlacementArea kontrolü
        GameObject placementArea = GameObject.Find("PlacementArea");
        if (placementArea != null)
        {
            Collider placementCollider = placementArea.GetComponent<Collider>();
            if (placementCollider != null)
            {
                // Gerekli iþlemleri buraya ekleyebilirsiniz.
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
