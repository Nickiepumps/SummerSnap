using UnityEngine;

public class NPCScoreEvaluator : MonoBehaviour
{
    public int EvaluateTotalNPCPenalty()
    {
        NPCScoringZoneChecker[] npcs = FindObjectsOfType<NPCScoringZoneChecker>();

        int totalPenalty = 0;

        foreach (var npc in npcs)
        {
            var zone = npc.CurrentZone;

            if (zone == NPCScoringZoneChecker.ZoneColor.None)
                continue;

            int layer = npc.GetComponent<SpriteRenderer>().sortingOrder;
            int penalty = 0;

            switch (zone)
            {
                case NPCScoringZoneChecker.ZoneColor.Green:
                    penalty = 2;
                    break;
                case NPCScoringZoneChecker.ZoneColor.Orange:
                    penalty = 5;
                    break;
                case NPCScoringZoneChecker.ZoneColor.Red:
                    penalty = (layer == 2) ? 5 : 10;
                    break;
            }

            Debug.Log($"{npc.name} อยู่ใน {zone} | Layer: {layer} | หักคะแนน: {penalty}");

            totalPenalty += penalty; // ✅ รวมคะแนนที่หักทุกตัว
        }

        return totalPenalty;
    }
}
