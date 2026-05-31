using UnityEngine;

[System.Serializable]
public class ParallaxBoundary : MonoBehaviour
{
    public float leftBound = -10f;
    public float rightBound = 10f;

    public bool IsWithinBounds(float newX)
    {
        return newX >= leftBound && newX <= rightBound;
    }

    public float ClampX(float newX)
    {
        return Mathf.Clamp(newX, leftBound, rightBound);
    }
}
