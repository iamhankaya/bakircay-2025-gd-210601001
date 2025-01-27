using UnityEngine;

public class TimeStopActivator : MonoBehaviour
{
    [SerializeField]
    private Skill gameSkill;  // GameSkill referansý

    [SerializeField]
    private float stopDuration = 5f; // Zaman durma süresi

    public void ActivateTimeStopper()
    {
        if (gameSkill != null)
        {
            gameSkill.ApplyTimeStopper(stopDuration); // Zaman durdur
        }
        else
        {
            Debug.LogError("GameSkill referansý bulunamadý!");
        }
    }
}
