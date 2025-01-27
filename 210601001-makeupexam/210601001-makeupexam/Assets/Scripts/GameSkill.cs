using UnityEngine;
using TMPro;

public class MatchTimer : MonoBehaviour
{
    [SerializeField]
    private PlacementController placementController; // PlacementController referans�

    [SerializeField]
    private TMPro.TextMeshProUGUI timerText; // S�reyi g�sterecek TextMeshProUGUI

    private float timer = 30f; // Ba�lang��ta 30 saniye verilecek
    private bool isTimerRunning = false;
    private int matchesMade = 0; // Yap�lan e�le�me say�s�

    private void Start()
    {
        // Timer ba�latma �rne�i (oyun ba�lad���nda)
        StartTimer();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime; // Zaman� d���r
            UpdateTimerText(); // Zaman� ekranda g�ster

            if (timer <= 0f)
            {
                StopTimer();
                EndGame(); // S�re bitince oyunu bitir
            }
        }
    }

    public void StartTimer()
    {
        if (!isTimerRunning) // Timer zaten �al���yorsa tekrar ba�latmay�n
        {
            timer = 30f; // Timer'� s�f�rla
            isTimerRunning = true;
            matchesMade = 0; // Ba�lang��ta e�le�me say�s�n� s�f�rla
            UpdateTimerText();
            Debug.Log($"Timer started at: {timer:F2}"); // Timer ba�lat�ld���nda log yazd�r�yoruz
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log($"Timer stopped at: {timer:F2}"); // Timer durduruldu�unda log yazd�r�yoruz
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {timer:F2}";
        }
    }

    private void EndGame()
    {
        // S�re bitiminde oyunun sonlanmas�n� sa�la
        Debug.Log("S�re bitti! Oyun sona erdi.");

        // Burada oyuncunun toplam puan� hesaplanabilir
        int finalScore = placementController.Score + (matchesMade * 10); // Her e�le�me i�in 10 puan ekle
        Debug.Log($"Toplam Skor: {finalScore}");

        // Oyun bitince skoru ekranda g�sterebilir veya yeni bir oyun ba�latabilirsiniz
    }

    // E�le�me yap�ld���nda �a�r�lacak fonksiyon
    public void OnMatchMade()
    {
        matchesMade++;
        Debug.Log($"E�le�me yap�ld�! Yap�lan e�le�meler: {matchesMade}");
    }
}
