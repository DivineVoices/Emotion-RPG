using System.Collections.Generic;
using UnityEngine;

public class GemModifier : MonoBehaviour
{
    public void ChangeGem(GemType gemType, int gemSlot = 0)
    {
        if (gemSlot < 0) gemSlot = 0;
        if (gemSlot > 2) gemSlot = 2;

        // Only equip if player owns this gem type
        if (!GemInventory.HasGem(gemType))
            return;

        switch (gemSlot)
        {
            case 0:
                GemInventory.firstGemType = gemType;
                Debug.Log("Sucessfully Set");
                break;
            case 1:
                GemInventory.secondGemType = gemType;
                Debug.Log("Sucessfully Set");
                break;
            case 2:
                GemInventory.thirdGemType = gemType;
                Debug.Log("Sucessfully Set");
                break;
        }
    }

    public void UpgradeGem(GemType gemType)
    {
        if (!GemInventory.gemsOwned.ContainsKey(gemType))
            return;
        GemLevel currentLevel = GemInventory.gemsOwned[gemType];
        GemLevel upgradedLevel = GetNextTier(currentLevel);
        

        if (upgradedLevel != GemLevel.None)
        {
            GemInventory.gemsOwned[gemType] = upgradedLevel;
            // No need to update equipped gems since they reference the type, not the level
        }
    }

    private GemLevel GetNextTier(GemLevel currentLevel)
    {
        switch (currentLevel)
        {
            case GemLevel.Level1: return GemLevel.Level2;
            case GemLevel.Level2: return GemLevel.Level3;
            case GemLevel.Level3: return GemLevel.Level3; // Max level, can't upgrade further
            default: return GemLevel.None;
        }
    }

    public void AddGemToInventory(GemType gemType, GemLevel level = GemLevel.Level1)
    {
        if (GemInventory.gemsOwned.ContainsKey(gemType))
        {
            // If already exists, keep the higher level
            GemLevel currentLevel = GemInventory.gemsOwned[gemType];
            if ((int)level > (int)currentLevel)
            {
                Debug.Log("Gem added");
                GemInventory.gemsOwned[gemType] = level;
            }
        }
        else
        {
            GemInventory.gemsOwned.Add(gemType, level);
        }
    }

    public void ClearEquippedGems()
    {
        GemInventory.firstGemType = GemType.None;
        GemInventory.secondGemType = GemType.None;
        GemInventory.thirdGemType = GemType.None;
    }

    public void ClearInventory()
    {
        GemInventory.gemsOwned.Clear();
        ClearEquippedGems();
    }

    // Helper method to get the equipped gem level for UI display
    public GemLevel GetEquippedGemLevel(int gemSlot)
    {
        if (gemSlot < 0 || gemSlot > 2) return GemLevel.None;

        GemType equippedType = GemType.None;
        switch (gemSlot)
        {
            case 0: equippedType = GemInventory.firstGemType; break;
            case 1: equippedType = GemInventory.secondGemType; break;
            case 2: equippedType = GemInventory.thirdGemType; break;
        }

        return equippedType == GemType.None ? GemLevel.None : GemInventory.GetGemLevel(equippedType);
    }

    // Helper method to get all owned gem types and their levels
    public Dictionary<GemType, GemLevel> GetAllOwnedGems()
    {
        return new Dictionary<GemType, GemLevel>(GemInventory.gemsOwned);
    }
}