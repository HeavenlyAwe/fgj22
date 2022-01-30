using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionSizeCounter : MonoBehaviour
{
    private static Dictionary<AIController.Faction, int> counts = new Dictionary<AIController.Faction, int>();

    public static void Add(AIController.Faction faction)
    {
        if (!counts.ContainsKey(faction))
        {
            counts.Add(faction, 0);
        }
        counts[faction]++;
    }

    public static void Remove(AIController.Faction faction)
    {
        if (!counts.ContainsKey(faction))
        {
            counts.Add(faction, 0);
        }
        counts[faction]--;
    }

    public static int Value(AIController.Faction faction)
    {
        if (counts.ContainsKey(faction))
        {
            return counts[faction];
        }
        return 0;
    }
}
