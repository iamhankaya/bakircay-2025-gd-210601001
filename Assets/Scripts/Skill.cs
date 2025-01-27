using UnityEngine;
using TMPro;

public class Skill : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI boostCountText;
    [SerializeField] private TextMeshProUGUI stopCountText;
    [SerializeField] private TextMeshProUGUI doubleCountText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    private float timer = 30f;
    private bool isTimerRunning = false;
    private bool isGameOver = false;
    private int matchesMade = 0;

    private bool isTimeStopped = false;
    private bool isDoublePointsActive = false;

    [SerializeField] private int initialBoostCount = 3;
    [SerializeField] private int initialStopCount = 3;
    [SerializeField] private int initialDoubleCount = 3;

    private int boostCount;
    private int stopCount;
    private int doubleCount;

    private void Start()
    {
        boostCount = initialBoostCount;
        stopCount = initialStopCount;
        doubleCount = initialDoubleCount;
        StartTimer();
    }

    private void Update()
    {
        if (isTimerRunning && !isGameOver && !isTimeStopped)
        {
            timer -= Time.deltaTime;
            UpdateTimerText();

            if (timer <= 0f)
            {
                StopTimer();
                EndGame();
            }
        }

        UpdateCounters();
    }

    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            timer = 30f;
            isTimerRunning = true;
            matchesMade = 0;
            isGameOver = false;
            UpdateTimerText();
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {timer:F2}";
        }
    }

    private void UpdateCounters()
    {
        if (boostCountText != null)
        {
            boostCountText.text = $"{boostCount}";
        }
        if (stopCountText != null)
        {
            stopCountText.text = $"{stopCount}";
        }
        if (doubleCountText != null)
        {
            doubleCountText.text = $"{doubleCount}";
        }
    }

    private void EndGame()
    {
        isGameOver = true;
        DisablePlayerInteractions();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {matchesMade}";
        }
    }

    private void DisablePlayerInteractions()
    {
        var interactableObjects = GameObject.FindObjectsOfType<MonoBehaviour>();

        foreach (var obj in interactableObjects)
        {
            if (obj is Dragger dragger)
            {
                dragger.enabled = false;
            }

            var rigidbody = obj.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }

            var collider = obj.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }

        Debug.Log("T�m kullan�c� aktiviteleri durduruldu.");
    }

    public void OnMatchMade()
    {
        if (!isGameOver)
        {
            matchesMade++;
        }
    }

    // Boost butonu i�levi
    public void ApplyTimeBoost(float boostAmount)
    {
        if (boostCount > 0)
        {
            boostCount--;
            ApplyBoost(boostAmount);
            UpdateCounters();  // UI'yi g�ncelle
            if (boostCount == 0) DisableBoostButton();
        }
    }

    public void ApplyTimeStopper(float duration)
    {
        if (stopCount > 0)
        {
            stopCount--;
            isTimeStopped = true;
            Invoke("ResumeTime", duration);
            UpdateCounters();  // UI'yi g�ncelle
            if (stopCount == 0) DisableStopButton();
        }
    }

    public void ApplyDoublePoints(float duration)
    {
        if (!isGameOver && !isDoublePointsActive && doubleCount > 0)
        {
            doubleCount--;
            isDoublePointsActive = true;
            UpdateCounters();  // UI'yi g�ncelle
            Debug.Log("�ift Puan Aktif!");
            Invoke("DeactivateDoublePoints", duration); // Verilen s�re kadar �ift puan etkin
            if (doubleCount == 0) DisableDoubleButton();
        }
    }

    // Zaman art�rma fonksiyonu
    public void ApplyBoost(float boostAmount)
    {
        if (!isGameOver && !isTimeStopped)
        {
            timer += boostAmount;
            UpdateTimerText();
        }
    }

    private void ResumeTime()
    {
        isTimeStopped = false;
        Debug.Log("Zaman yeniden ba�lad�.");
    }

    private void DeactivateDoublePoints()
    {
        isDoublePointsActive = false;
        Debug.Log("�ift Puan devre d��� b�rak�ld�.");
    }

    private void DisableBoostButton()
    {
        Debug.Log("Boost butonu devre d��� b�rak�ld�.");
    }

    private void DisableStopButton()
    {
        Debug.Log("Stop butonu devre d��� b�rak�ld�.");
    }

    private void DisableDoubleButton()
    {
        Debug.Log("Double butonu devre d��� b�rak�ld�.");
    }

    // Oyunu tamamen s�f�rlama fonksiyonu
    public void ResetTimer()
    {
        // Timer'� s�f�rla
        timer = 30f;
        isTimerRunning = false;

        // Boost, Stop ve Double say�lar� s�f�rlanacak
        boostCount = initialBoostCount;
        stopCount = initialStopCount;
        doubleCount = initialDoubleCount;

        // Timer'� ve saya�lar� g�ncelle
        UpdateTimerText();
        UpdateCounters();

        // Oyun bitti bayra��n� s�f�rla
        isGameOver = false;
        matchesMade = 0;

        // Oyun yeniden ba�lat�lacak
        StartTimer();
    }
}
