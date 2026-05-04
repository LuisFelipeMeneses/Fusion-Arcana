using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class SpellSO : ScriptableObject
{
    [SerializeField] protected string spellName = "Spell";
    [SerializeField] protected float xSpeed = 1;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected SpellSO[] strongWith;
    [SerializeField] protected AnimatorOverrideController animations;
    [SerializeField] protected AnimatorOverrideController hitAnimations;
    [SerializeField] protected Sprite imageHUD;
    [SerializeField] protected SpellBehavior[] behaviors;

    public float XSpeed => xSpeed;
    public int Damage => damage;
    public SpellSO[] StrongWith => strongWith;
    public AnimatorOverrideController Animations => animations;
    public AnimatorOverrideController HitAnimations => hitAnimations;
    public Sprite ImageHUD => imageHUD;
    public SpellBehavior[] Behaviors => behaviors;

    /*
    public abstract void OnStart(SpellScript spell);
    public abstract void OnUpdate(SpellScript spell);
    public abstract void OnDie(SpellScript spell);
    public virtual void OnCollide(SpellScript spell, Collider2D other)
    {
        SpellScript otherSpell = other.gameObject.GetComponent<SpellScript>();

        if (otherSpell != null && strongWith.Contains(otherSpell.SpellData))
        {
            otherSpell.Die();
        }

        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
        if (player != null && spell.Conjurer != player.gameObject)
        {
            spell.Hit();
        }
    }

    */


    public override string ToString()
    {
        return spellName;
    }
}
