using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.U2D;

public class SpellHitScript : MonoBehaviour
{
    private AnimatorOverrideController animations;
    private Animator anim;
    private Vector2 direction;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = animations;
        if (direction == Vector2.left)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
            !anim.IsInTransition(0))
        {
            Destroy(gameObject);
        }
    }

    public void Init(AnimatorOverrideController animation, Vector2 dir)
    {
        animations = animation;
        direction = dir;
    }
}
