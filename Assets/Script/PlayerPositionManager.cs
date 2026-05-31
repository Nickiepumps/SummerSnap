using UnityEngine;

public static class PlayerPositionManager
{
    private static Vector3 savedPosition;
    private static bool hasSaved = false;

    public static void SavePlayerPosition(Vector3 pos)
    {
        savedPosition = pos;
        hasSaved = true;
    }

    public static bool HasSavedPosition()
    {
        return hasSaved;
    }

    public static Vector3 RestorePlayerPosition()
    {
        hasSaved = false; // ใช้แล้วเคลียร์ค่า
        return savedPosition;
    }

   
}
