using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFactionCounts : MonoBehaviour
{
    public Text fireText;
    public Text waterText;

    // Update is called once per frame
    void Update()
    {
        waterText.text = "Water count: " + FactionSizeCounter.Value(AIController.Faction.Water);
        fireText.text = "Fire count: " + FactionSizeCounter.Value(AIController.Faction.Fire);
    }
}
