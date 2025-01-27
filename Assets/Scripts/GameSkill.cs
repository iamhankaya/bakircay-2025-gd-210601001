using UnityEngine;
using TMPro;

public class MatchTimer : MonoBehaviour
{
    [SerializeField]
    private PlacementController placementController; // PlacementController referansý

    [SerializeField]
    private TMPro.TextMeshProUGUI timerText; // Süreyi gösterecek TextMeshProUGUI

    private float timer = 30f; // Baþlangýçta 30 saniye verilecek
    private bool isTimerRunning = false;
    private int matchesMade = 0; // Yapýlan eþleþme sayýsý

    private void Start()
    {
        // Timer baþlatma örneði (oyun baþladýðýnda)
        StartTimer();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime; // Zamaný düþür
            UpdateTimerText(); // Zamaný ekranda göster

            if (timer <= 0f)
            {
                StopTimer();
                EndGame(); // Süre bitince oyunu bitir
            }
        }
    }

    public void StartTimer()
    {
        if (!isTimerRunning) // Timer zaten çalýþýyorsa tekrar baþlatmayýn
        {
            timer = 30f; // Timer'ý sýfýrla
            isTimerRunning = true;
            matchesMade = 0; // Baþlangýçta eþleþme sayýsýný sýfýrla
            UpdateTimerText();
            Debug.Log($"Timer started at: {timer:F2}"); // Timer baþlatýldýðýnda log yazdýrýyoruz
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log($"Timer stopped at: {timer:F2}"); // Timer durdurulduðunda log yazdýrýyoruz
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
        // Süre bitiminde oyunun sonlanmasýný saðla
        Debug.Log("Süre bitti! Oyun sona erdi.");

        // Burada oyuncunun toplam puaný hesaplanabilir
        int finalScore = placementController.Score + (matchesMade * 10); // Her eþleþme için 10 puan ekle
        Debug.Log($"Toplam Skor: {finalScore}");

        // Oyun bitince skoru ekranda gösterebilir veya yeni bir oyun baþlatabilirsiniz
    }

    // Eþleþme yapýldýðýnda çaðrýlacak fonksiyon
    public void OnMatchMade()
    {
        matchesMade++;
        Debug.Log($"Eþleþme yapýldý! Yapýlan eþleþmeler: {matchesMade}");
    }
}
