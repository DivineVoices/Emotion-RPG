using UnityEngine;
using TMPro;

public class KarmaDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textSpot;

    public void DisplayKarma()
    {
        textSpot.gameObject.SetActive(true);
        textSpot.text = Karma.karmaGauge.ToString();
    }

    public void ModifKarma(int karma)
    {
        Karma.karmaGauge += karma;
    }

    public void ClearKarmaDisplay()
    {
        textSpot.gameObject.SetActive(false);
    }
}
