using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GemInventory
{
    public static Dictionary<GemType, GemLevel> gemsOwned = new Dictionary<GemType, GemLevel>();
    public static GemType firstGemType = GemType.None;
    public static GemType secondGemType = GemType.None;
    public static GemType thirdGemType = GemType.None;

    public static GemLevel GetGemLevel(GemType gemType)
    {
        if (gemsOwned.ContainsKey(gemType))
            return gemsOwned[gemType];
        return GemLevel.None;
    }

    public static bool HasGem(GemType gemType)
    {
        return gemsOwned.ContainsKey(gemType) && gemsOwned[gemType] != GemLevel.None;
    }
}


public static class GemChecker
{
    public static bool HasEquippedGem(GemType gemType, GemLevel minLevel = GemLevel.Level1) // Check if 
    {
        return (GemInventory.firstGemType == gemType && GetEquippedGemLevel(0) >= minLevel) ||
               (GemInventory.secondGemType == gemType && GetEquippedGemLevel(1) >= minLevel) ||
               (GemInventory.thirdGemType == gemType && GetEquippedGemLevel(2) >= minLevel);
    }

    public static bool HasEquippedGemExact(GemType gemType, GemLevel exactLevel)
    {
        return (GemInventory.firstGemType == gemType && GetEquippedGemLevel(0) == exactLevel) ||
               (GemInventory.secondGemType == gemType && GetEquippedGemLevel(1) == exactLevel) ||
               (GemInventory.thirdGemType == gemType && GetEquippedGemLevel(2) == exactLevel);
    }

    public static int CountEquippedGems(GemType gemType, GemLevel minLevel = GemLevel.Level1)
    {
        int count = 0;
        if (GemInventory.firstGemType == gemType && GetEquippedGemLevel(0) >= minLevel) count++;
        if (GemInventory.secondGemType == gemType && GetEquippedGemLevel(1) >= minLevel) count++;
        if (GemInventory.thirdGemType == gemType && GetEquippedGemLevel(2) >= minLevel) count++;
        return count;
    }

    private static GemLevel GetEquippedGemLevel(int slot)
    {
        GemType gemType = slot switch
        {
            0 => GemInventory.firstGemType,
            1 => GemInventory.secondGemType,
            2 => GemInventory.thirdGemType,
            _ => GemType.None
        };
        return gemType == GemType.None ? GemLevel.None : GemInventory.GetGemLevel(gemType);
    }
}

// Static extension methods (already work without instances)
public static class GemExtensions
{
    public static bool IsEquipped(this GemType gemType, GemLevel minLevel = GemLevel.Level1)
    {
        return GemChecker.HasEquippedGem(gemType, minLevel);
    }

    public static int EquippedCount(this GemType gemType, GemLevel minLevel = GemLevel.Level1)
    {
        return GemChecker.CountEquippedGems(gemType, minLevel);
    }
}

public static class GemTypeConverter
{
    public static GemType FromString(string gemString)
    {
        if (string.IsNullOrEmpty(gemString))
            return GemType.None;

        // Remove any whitespace and convert to proper case
        string cleanedString = gemString.Trim();

        // Try exact match first
        if (System.Enum.TryParse<GemType>(cleanedString, true, out GemType result))
        {
            return result;
        }

        // Handle common variations and aliases
        return cleanedString.ToLower() switch
        {
            "amethyst" or "purple" or "ameth" => GemType.Amethyst,
            "topaz" or "yellow" or "top" => GemType.Topaz,
            "ruby" or "red" => GemType.Ruby,
            "none" or "empty" or "" or "null" => GemType.None,
            _ => GemType.None // Default to None if no match found
        };
    }

    public static bool TryFromString(string gemString, out GemType gemType)
    {
        gemType = FromString(gemString);
        return gemType != GemType.None || gemString?.ToLower() == "none";
    }

    public static string ToString(GemType gemType)
    {
        return gemType.ToString();
    }
}

public enum GemType
{
    None,
    Amethyst,
    Topaz,
    Ruby,
}

public enum GemLevel
{
    None,
    Level1,
    Level2,
    Level3
}
