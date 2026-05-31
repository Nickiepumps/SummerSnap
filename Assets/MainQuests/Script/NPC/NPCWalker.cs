using UnityEngine;

public class NPCWalker : MonoBehaviour
{
    public float speed = 2f;
    public float waitTime = 1f;
    public Vector3 startPoint;
    public Vector3 endPoint;

    private bool movingToEnd = true;
    private bool waiting = false;

    private bool isFacingRight = true;

    void Start()
    {
        transform.position = startPoint;
        StartCoroutine(WaitBeforeMoving());
    }

    void Update()
    {
        if (waiting) return;

        Vector3 target = movingToEnd ? endPoint : startPoint;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Flip ด้วย localScale แทน flipX
        if (movingToEnd && !isFacingRight)
        {
            FlipSprite();
        }
        else if (!movingToEnd && isFacingRight)
        {
            FlipSprite();
        }

        // เปลี่ยนทิศเมื่อถึงจุด
        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            waiting = true;
            movingToEnd = !movingToEnd;
            StartCoroutine(WaitBeforeMoving());
        }
    }

    private void FlipSprite()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

        isFacingRight = !isFacingRight;
    }

    System.Collections.IEnumerator WaitBeforeMoving()
    {
        yield return new WaitForSeconds(waitTime);
        waiting = false;
    }
}
