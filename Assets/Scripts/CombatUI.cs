using UnityEngine; 
using UnityEngine.UI;

public class AttackSystem : MonoBehaviour
{
    int karma;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform gridParent;
    int totalLevel = 0;
    int GemCount = 0;

    void Start()
    {
        GemCount = GemChecker.CountEquippedGems();
        if(!(GemInventory.firstGemType == GemType.None))
        {
            totalLevel += GemInventory.GetGemLevelIndex(GemInventory.firstGemType);
        }
        if(!(GemInventory.secondGemType == GemType.None))
        {
            totalLevel += GemInventory.GetGemLevelIndex(GemInventory.secondGemType);
        }
        if(!(GemInventory.thirdGemType == GemType.None))
        {
            totalLevel += GemInventory.GetGemLevelIndex(GemInventory.thirdGemType);
        }
        for (int i = 0; i < totalLevel; i++) 
        {
            Instantiate(buttonPrefab.GetComponent<Button>(), gridParent);
        }
    }

    public void Attack(GemType type, GemLevel level, EmotionGaugeManager gaugeManager)
    {
        gaugeManager.EmotionGauge += 2;
    }
}
