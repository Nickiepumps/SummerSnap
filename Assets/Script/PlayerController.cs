using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    private Animator animator;
    private Vector2 movement;
    private bool isFacingRight = true;

    public AudioSource footstepAudio;
    public bool hasPowder = false;

    [Header("Animator Controllers - Player")]
    public RuntimeAnimatorController controllerNoPowder;
    public RuntimeAnimatorController controllerWithPowder;

    public bool canMove = true;
    public bool isOnDialogue {  get; private set; } = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // ลบ static flag ออก
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        if (GameManager.Instance != null)
        {
            var data = GameManager.Instance.playerData;

            transform.position = data.lastPosition;
            hasPowder = data.hasPowder;
            animator.runtimeAnimatorController = hasPowder ? controllerWithPowder : controllerNoPowder;

            // ให้ Ryu sync ตั้งแต่เริ่ม
            var ryuAnimator = GameManager.Instance.GetRyuAnimator();
            if (ryuAnimator != null)
            {
                ryuAnimator.runtimeAnimatorController = GameManager.Instance.GetRyuPowderController(hasPowder);
            }
        }
        else
        {
            animator.runtimeAnimatorController = controllerNoPowder;
        }
    }

    void Update()
    {
        if (!canMove)
        {
            animator.SetInteger("PlayerAct", isFacingRight ? 0 : 1);
            footstepAudio.Stop();
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        movement.x = horizontalInput;

        if (horizontalInput < 0)
        {
            animator.SetInteger("PlayerAct", 3);
            isFacingRight = false;
        }
        else if (horizontalInput > 0)
        {
            animator.SetInteger("PlayerAct", 2);
            isFacingRight = true;
        }
        else
        {
            animator.SetInteger("PlayerAct", isFacingRight ? 0 : 1);
        }

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            if (!footstepAudio.isPlaying)
                footstepAudio.Play();
        }
        else footstepAudio.Stop();

        if (AudioManager.Instance != null && footstepAudio != null)
        {
            footstepAudio.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }

        MovePlayer();
    }

    void MovePlayer()
    {
        if(isOnDialogue == false)
        {
            transform.Translate(movement.x * moveSpeed * Time.deltaTime, 0, 0);
        }
    }

    public void SetPowderOnFace(bool value)
    {
        if (value && !hasPowder)
        {
            hasPowder = true;
            animator.runtimeAnimatorController = controllerWithPowder;

            if (GameManager.Instance != null)
            {
                var ryuAnimator = GameManager.Instance.GetRyuAnimator();
                if (ryuAnimator != null)
                {
                    ryuAnimator.runtimeAnimatorController = GameManager.Instance.GetRyuPowderController(true);
                }
            }
        }
    }
    public void TogglePlayerDialogue(bool state)
    {
        isOnDialogue = state;
        canMove = !state;
    }
    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            var data = GameManager.Instance.playerData;
            data.lastPosition = transform.position;
            data.hasPowder = hasPowder;
        }
    }
}
