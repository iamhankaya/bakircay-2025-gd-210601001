using UnityEngine;

public class TimeBoostActivator : MonoBehaviour
{
    [SerializeField]
    private Skill gameSkill;  // GameSkill referansý

    [SerializeField]
    private float boostAmount = 10f; // Zaman artýþý miktarý

    public void ActivateTimeBoost()
    {
        if (gameSkill != null)
        {
            gameSkill.ApplyTimeBoost(boostAmount); // Zamaný artýr
        }
        else
        {
            Debug.LogError("GameSkill referansý bulunamadý!");
        }
    }
}
