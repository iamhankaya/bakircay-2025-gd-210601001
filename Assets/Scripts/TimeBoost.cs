using UnityEngine;

public class TimeBoostActivator : MonoBehaviour
{
    [SerializeField]
    private Skill gameSkill;  // GameSkill referans�

    [SerializeField]
    private float boostAmount = 10f; // Zaman art��� miktar�

    public void ActivateTimeBoost()
    {
        if (gameSkill != null)
        {
            gameSkill.ApplyTimeBoost(boostAmount); // Zaman� art�r
        }
        else
        {
            Debug.LogError("GameSkill referans� bulunamad�!");
        }
    }
}
