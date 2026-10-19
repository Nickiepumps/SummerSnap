using Cysharp.Threading.Tasks;
using UnityEngine;
using PrimeTween;
public class NPCBirdController : MonoBehaviour
{
    [SerializeField] private Collider2D birdCollider;
    [SerializeField] private Animator birdAnimator;

    [SerializeField] private SpriteRenderer birdSprite;

    [SerializeField] private Transform takeOffTarget;
    [SerializeField] private Transform landingTarget;
    [SerializeField] private Transform startTarget;

    public bool ignorePlayer;
    public bool isLanding;
    private void Start()
    {
        if(isLanding == true)
        {
            birdSprite.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" )
        {
            if(ignorePlayer == false && isLanding == false)
            {
                StartBirdTakeOffAnimation().Forget();
            }
            else if(ignorePlayer == false && isLanding == true)
            {
                StartBirdLandingAnimation().Forget();
            }
        }
    }
    private async UniTaskVoid StartBirdLandingAnimation()
    {
        birdCollider.enabled = false;
        birdSprite.gameObject.transform.position = startTarget.position;
        birdSprite.enabled = true;
        birdAnimator.SetBool("IsLanding", true);
        birdAnimator.SetBool("IsTakeOff", false);
        await Tween.Position(birdSprite.gameObject.transform, landingTarget.position, 1.4f, Easing.Standard(Ease.OutCubic));
        birdAnimator.SetBool("IsLanding", false);
        birdAnimator.SetBool("IsTakeOff", false);
    }
    private async UniTaskVoid StartBirdTakeOffAnimation()
    {
        birdCollider.enabled = false;
        birdAnimator.SetBool("IsLanding", false);
        birdAnimator.SetBool("IsTakeOff", true);
        await Tween.Position(birdSprite.gameObject.transform, takeOffTarget.position, 1f, Easing.Standard(Ease.InCubic));
    }
}
