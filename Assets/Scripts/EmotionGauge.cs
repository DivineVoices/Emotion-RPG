using UnityEngine;

public class EmotionGaugeManager : MonoBehaviour
{
    [SerializeField] int _emotionGauge;
    public int EmotionGauge { get => _emotionGauge; set => _emotionGauge = value; }

    public enum Gems
    {
        None,
        Amethyst,
        Topaz,
        Ruby,
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
