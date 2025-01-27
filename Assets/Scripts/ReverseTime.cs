using UnityEngine;

public class DoublePointsButton : MonoBehaviour
{
    [SerializeField]
    private Skill gameSkill; // GameSkill de�il, Skill script'i kullan�yoruz

    [SerializeField]
    private float doublePointsDuration = 10f; // Double points etkinli�inin s�resi (saniye)

    public void OnButtonPressed()
    {
       if (gameSkill != null)
       {
            gameSkill.ApplyDoublePoints(doublePointsDuration); // Double Points fonksiyonunu �a��r ve s�resini ilet
        }
    }
}
