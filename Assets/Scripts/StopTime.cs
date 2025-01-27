using UnityEngine;

public class TimeStopActivator : MonoBehaviour
{
    [SerializeField]
    private Skill gameSkill;  // GameSkill referans�

    [SerializeField]
    private float stopDuration = 5f; // Zaman durma s�resi

    public void ActivateTimeStopper()
    {
        if (gameSkill != null)
        {
            gameSkill.ApplyTimeStopper(stopDuration); // Zaman durdur
        }
        else
        {
            Debug.LogError("GameSkill referans� bulunamad�!");
        }
    }
}
