using UnityEngine;

public class DoublePointsButton : MonoBehaviour
{
    [SerializeField]
    private Skill gameSkill; // GameSkill deðil, Skill script'i kullanýyoruz

    [SerializeField]
    private float doublePointsDuration = 10f; // Double points etkinliðinin süresi (saniye)

    public void OnButtonPressed()
    {
       if (gameSkill != null)
       {
            gameSkill.ApplyDoublePoints(doublePointsDuration); // Double Points fonksiyonunu çaðýr ve süresini ilet
        }
    }
}
