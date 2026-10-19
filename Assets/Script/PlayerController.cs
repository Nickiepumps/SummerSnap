using UnityEngine;
using PrimeTween;

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

    public GameObject phoneNotif;
    public SpriteRenderer phoneNotifSprite;
    Sequence notifTweenSequence;

    public bool canMove = true;
    public bool isOnDialogue {  get; private set; } = false;
    public bool isOnSideQuest = false;
    public bool isWalking = false;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // ลบ static flag ออก
    }

    private void Start()
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
    private void Update()
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
            isWalking = true;
        }
        else if (horizontalInput > 0)
        {
            animator.SetInteger("PlayerAct", 2);
            isFacingRight = true;
            isWalking = true;
        }
        else
        {
            animator.SetInteger("PlayerAct", isFacingRight ? 0 : 1);
            isWalking = false;
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
    private void MovePlayer()
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
        isOnSideQuest = state;
        canMove = !state;
    }
    public void TogglePhoneNotif(bool state)
    {
        if (state == true && phoneNotif.activeSelf == true) return;

        if(state == false)
        {
            notifTweenSequence.Stop();
            phoneNotif.SetActive(false);
        }
        else
        {
            notifTweenSequence = Sequence.Create()
                .Group(Tween.Scale(phoneNotif.transform, Vector3.zero, Vector3.one, 0.8f, Easing.Standard(Ease.OutBack)))
                .ChainCallback(() => Tween.Alpha(phoneNotifSprite, 1f, 0f, 0.5f, Easing.Standard(Ease.OutCubic), cycles: -1, cycleMode: CycleMode.Yoyo));
            phoneNotif.SetActive(true);
        }
    }
    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            var data = GameManager.Instance.playerData;
            data.lastPosition = transform.position;
            data.hasPowder = hasPowder;
        }
    }
}
