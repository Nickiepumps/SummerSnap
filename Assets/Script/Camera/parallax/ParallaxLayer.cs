using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;
    private ParallaxBoundary boundary;

    void Start()
    {
        boundary = GetComponent<ParallaxBoundary>();
    }

    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta * parallaxFactor;

        if (boundary != null)
        {
            newPos.x = boundary.ClampX(newPos.x);
        }

        transform.localPosition = newPos;
    }
}
