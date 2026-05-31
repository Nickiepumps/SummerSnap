using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NPCPanel : MonoBehaviour
{
    [Header("Waypoints Settings")]
    public Transform[] waypoints;
    public float speed = 2f;
    public bool loop = false;
    public float waitTime = 1f;

    [Header("Footstep Sound")]
    public AudioClip footstepClip;
    private AudioSource footstepAudio;

    private int currentIndex = 0;
    private bool isWalking = true;
    private float waitCounter = 0f;
    private SpriteRenderer sr;
    private float fixedY;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();

        fixedY = transform.position.y;

        // ตั้งค่า AudioSource (ถ้าไม่มี)
        if (footstepAudio == null)
            footstepAudio = gameObject.AddComponent<AudioSource>();

        footstepAudio.clip = footstepClip;
        footstepAudio.loop = true;
        footstepAudio.playOnAwake = false;

        // 👇 ทำให้เสียงออกจากตัว NPC แบบ 3D
        footstepAudio.spatialBlend = 1f;   // 1 = 3D
        footstepAudio.minDistance = 1f;    // ระยะที่เสียงดังเต็ม
        footstepAudio.maxDistance = 10f;   // ระยะที่เสียงเริ่มหายไป
        footstepAudio.rolloffMode = AudioRolloffMode.Linear; // ค่อยๆ เบาลงตามระยะ
    }


    void Update()
    {
        // Sync Volume กับ AudioManager ตลอดเวลา
        UpdateVolumeFromManager();

        if (waypoints == null || waypoints.Length == 0 || !isWalking)
        {
            StopFootstep();
            return;
        }

        if (waypoints.Length == 1)
        {
            if (waypoints[0] != null)
                transform.position = new Vector3(waypoints[0].position.x, fixedY, transform.position.z);
            StopFootstep();
            return;
        }

        if (waitCounter > 0f)
        {
            waitCounter -= Time.deltaTime;
            StopFootstep();
            return;
        }

        if (waypoints[currentIndex] == null)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                if (loop) currentIndex = 0;
                else { isWalking = false; StopFootstep(); return; }
            }
            return;
        }

        Transform target = waypoints[currentIndex];
        Vector3 targetPos = new Vector3(target.position.x, fixedY, transform.position.z);
        Vector2 direction = targetPos - transform.position;

        if (sr != null)
        {
            if (direction.x > 0.01f) sr.flipX = false;
            else if (direction.x < -0.01f) sr.flipX = true;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        PlayFootstep();

        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            waitCounter = waitTime;
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                if (loop) currentIndex = 0;
                else { isWalking = false; StopFootstep(); return; }
            }
        }
    }

    // -------------------------------
    // ฟังก์ชันเสียงเดิน
    // -------------------------------
    void PlayFootstep()
    {
        if (footstepClip == null) return;
        if (!footstepAudio.isPlaying)
            footstepAudio.Play();
    }

    void StopFootstep()
    {
        if (footstepAudio.isPlaying)
            footstepAudio.Stop();
    }

    // -------------------------------
    // Sync Volume จาก AudioManager
    // -------------------------------
    void UpdateVolumeFromManager()
    {
        if (AudioManager.Instance != null)
        {
            footstepAudio.volume = AudioManager.Instance.CurrentSFXVolume;

        }
    }
}
