using UnityEngine;
using UnityEngine.SceneManagement;

public class RyuController : MonoBehaviour
{
    public enum RyuState { Idle, Follow, Wander, Ending, Wait }

    [Header("References")]
    public Transform player;

    [Header("Audio")]
    public AudioClip laughClip; // ✅ เสียงหัวเราะ ริว
    private bool hasLaughed = false; // เล่นครั้งเดียวหลังมาถึงผู้เล่น

    [Header("Follow Settings")]
    public float followDistance = 3f;
    public float detectDistance = 6f;
    public float speed = 2f;
    private float baseSpeed;

    [Header("Call Settings")]
    public float callSpeedMultiplier = 2.5f;
    private bool isCalling = false;

    [Header("Wander Settings")]
    public float wanderDelay = 5f;
    public float wanderRange = 30f;

    [Header("Position Fix")]
    public float fixedY = -2.2f;

    [Header("Ending Settings")]
    public Transform endingSpawnPoint;
    public float idleToWanderDelay = 8f;
    public float endingStopDistance = 0.1f;
    public float endingSpeedMultiplier = 3f;

    private Vector2 wanderTarget;
    private float wanderTimer;

    private RyuState currentState = RyuState.Idle;
    private Rigidbody2D rb;
    private Animator animator;

    private float idleTimer;
    private int lastDirection = 1;

    public System.Action OnArrivedAtPlayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        baseSpeed = speed;

        if (player == null && GameManager.Instance != null)
            player = GameManager.Instance.GetPlayer().transform;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject obj = GameObject.Find("EndingSpawnPoint");
        endingSpawnPoint = obj != null ? obj.transform : null;
    }

    void Update()
    {
        if (player == null) return;

        if (GameManager.Instance != null && endingSpawnPoint != null)
        {
            int completedQuests = 0;
            if (GameManager.Instance.scoreLevel1 > 0) completedQuests++;
            if (GameManager.Instance.scoreLevel2 > 0) completedQuests++;
            if (GameManager.Instance.scoreLevel3 > 0) completedQuests++;
            if (GameManager.Instance.scoreLevel4 > 0) completedQuests++;

            if (completedQuests >= 4 && currentState != RyuState.Ending)
                currentState = RyuState.Ending;
        }

        switch (currentState)
        {
            case RyuState.Idle: HandleIdle(); break;
            case RyuState.Follow: HandleFollow(); break;
            case RyuState.Wander: HandleWander(); break;
            case RyuState.Ending: HandleEnding(); break;
            case RyuState.Wait: HandleWait(); break;
        }
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = fixedY;
        transform.position = pos;
    }

    void HandleIdle()
    {
        SetAnimation(lastDirection == 1 ? 1 : 0);
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < detectDistance)
            currentState = RyuState.Follow;
    }

    public bool IsCalling()
    {
        return isCalling;
    }

    public void CallRyu()
    {
        currentState = RyuState.Follow;
        isCalling = true;
        baseSpeed = speed;
        hasLaughed = false; // รีเซ็ตเพื่อเล่นเสียงหัวเราะใหม่หลังโทร
    }

    void HandleFollow()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        Vector2 targetPos = new Vector2(player.position.x, transform.position.y);

        if (distance > followDistance)
        {
            MoveTowards(targetPos, 0.5f);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            baseSpeed = speed;
            isCalling = false;

            // ✅ เล่นเสียงหัวเราะผ่าน AudioManager ครั้งเดียว
            if (!hasLaughed && laughClip != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(laughClip);
                hasLaughed = true;
            }

            OnArrivedAtPlayer?.Invoke();
            currentState = RyuState.Wait;
        }
    }

    void HandleWander()
    {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0)
        {
            float randomOffset = Random.Range(-wanderRange, wanderRange);
            float targetX = player.position.x + randomOffset;
            wanderTarget = new Vector2(targetX, transform.position.y);
            wanderTimer = wanderDelay;
        }
        MoveTowards(wanderTarget);
    }

    void HandleEnding()
    {
        if (endingSpawnPoint == null) return;

        float distance = Vector2.Distance(transform.position, endingSpawnPoint.position);
        if (distance > endingStopDistance)
            MoveTowards(endingSpawnPoint.position, endingSpeedMultiplier);
        else
        {
            rb.linearVelocity = Vector2.zero;
            SetIdleAnim();
        }
    }
    private void HandleWait()
    {
        if (Mathf.Abs(player.GetComponent<Rigidbody2D>().linearVelocity.x) < 0.01f)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleToWanderDelay)
                {
                    idleTimer = 0f;
                    currentState = RyuState.Wander;
                }
                else SetIdleAnim();
            }
            else
            {
                idleTimer = 0f;
                SetIdleAnim();
            }
    }
    void MoveTowards(Vector2 target, float multiplier = 1f)
    {
        float moveSpeed = (isCalling ? baseSpeed * callSpeedMultiplier : baseSpeed) * multiplier;
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.deltaTime);

        float direction = target.x - rb.position.x;
        if (direction > 0.01f) lastDirection = 1;
        else if (direction < -0.01f) lastDirection = -1;

        if(Vector2.Distance(rb.position, target) <= 0.2f)
        {
            SetIdleAnim();
        }
        else
        {
            rb.MovePosition(newPos);
            SetWalkAnim();
        }
    }

    void SetIdleAnim()
    {
        rb.linearVelocity = Vector2.zero;
        SetAnimation(lastDirection == 1 ? 1 : 0);
    }

    void SetWalkAnim()
    {
        SetAnimation(lastDirection == 1 ? 2 : 3);
    }
    void SetAnimation(int stateValue)
    {
        if (animator != null)
            animator.SetInteger("Ryu", stateValue);
    }
}
