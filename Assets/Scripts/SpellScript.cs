using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.U2D;

public class SpellScript : MonoBehaviour
{
    [SerializeField] private GameObject spellHitPrefab;
    [SerializeField] private SpellSO spellData;
    private AnimatorOverrideController animations;
    private AnimatorOverrideController hitAnimations;
    private GameObject conjurer;
    private Animator anim;
    private float xSpeed;
    private Vector2 direction;
    private Dictionary<SpellBehavior, SpellState> states = new();

    private System.Action onStart;
    private System.Action onUpdate;
    private System.Action<Collider2D> onCollide;
    private System.Action onDie;

    public GameObject Conjurer => conjurer;
    public SpellSO SpellData => spellData;
    public float XSpeed => xSpeed;
    public Vector2 Direction => direction;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = animations;
        if (direction == Vector2.left)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        onStart?.Invoke();
    }

    void Update()
    {
        onUpdate?.Invoke();
    }

    public void Instantiate(SpellSO data, Vector2 dir, GameObject conjurer)
    {
        this.conjurer = conjurer;
        spellData = data;
        xSpeed = data.XSpeed;
        direction = dir;
        animations = data.Animations;
        hitAnimations = data.HitAnimations;

        BuildDelegades();
    }

    private void BuildDelegades()
    {
        onStart = null;
        onUpdate = null;
        onCollide = null;
        onDie = null;

        var behaviors = spellData.Behaviors;
        foreach (var b in behaviors)
        {
            onStart += () => b.OnStart(this);
            onUpdate += () => b.OnUpdate(this);
            onCollide += (col) => b.OnCollide(this, col);
            onDie += () => b.OnDie(this);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        onCollide?.Invoke(collision);
    }

    public void Die()
    {
        onDie?.Invoke();
        Destroy(gameObject);
    }

    public void Hit()
    {
        GameObject spellHit = Instantiate(spellHitPrefab, transform.position, Quaternion.identity);
        SpellHitScript spellHitScript = spellHit.GetComponent<SpellHitScript>();
        spellHitScript.Init(hitAnimations, direction);
        Die();
    }

    public SpellState GetState(SpellBehavior behavior)
    {
        if (!states.TryGetValue(behavior, out var state))
        {
            state = new SpellState();
            states[behavior] = state;
        }

        return state;
    }

    public void Reflect(GameObject newConjurer)
    {
        conjurer = newConjurer;
        direction = -direction;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    public void ChangeSpeed(float newSpeed)
    {
        xSpeed = newSpeed;
    }

    public bool TryGetBehavior<T>(out T behavior) where T : SpellBehavior
    {
        behavior = SpellData.Behaviors
            .OfType<T>()
            .FirstOrDefault();

        return behavior != null;
    }
}
