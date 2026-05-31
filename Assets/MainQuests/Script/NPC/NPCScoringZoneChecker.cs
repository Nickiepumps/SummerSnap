using UnityEngine;
using System.Collections.Generic;

public class NPCScoringZoneChecker : MonoBehaviour
{
    public enum ZoneColor { None, Green, Orange, Red }

    private HashSet<ZoneColor> activeZones = new HashSet<ZoneColor>();

    public ZoneColor CurrentZone
    {
        get
        {
            if (activeZones.Contains(ZoneColor.Red)) return ZoneColor.Red;
            if (activeZones.Contains(ZoneColor.Orange)) return ZoneColor.Orange;
            if (activeZones.Contains(ZoneColor.Green)) return ZoneColor.Green;
            return ZoneColor.None;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ZoneGreen"))
            activeZones.Add(ZoneColor.Green);
        else if (other.CompareTag("ZoneOrange"))
            activeZones.Add(ZoneColor.Orange);
        else if (other.CompareTag("ZoneRed"))
            activeZones.Add(ZoneColor.Red);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ZoneGreen"))
            activeZones.Remove(ZoneColor.Green);
        else if (other.CompareTag("ZoneOrange"))
            activeZones.Remove(ZoneColor.Orange);
        else if (other.CompareTag("ZoneRed"))
            activeZones.Remove(ZoneColor.Red);
    }
}
