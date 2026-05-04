using UnityEngine;

[CreateAssetMenu(fileName = "ReflectionBehavior", menuName = "Spells/Behavior/New Reflection Behavior")]
public class ReflectionBehavior : SpellBehavior
{
    [SerializeField] private int durability = 1;
    [SerializeField] private bool isPure = true;

    public override void OnStart(SpellScript spell)
    {
        SpellState state = spell.GetState(this);
        state.durability = durability;
        state.isPure = isPure;
        if (state.isPure)
        {
            spell.transform.position = new Vector2(4 * -spell.Direction.x, 0.5f);
        }
    }

    public override void OnCollide(SpellScript spell, Collider2D other)
    {
        SpellScript otherSpell = other.GetComponent<SpellScript>();
        if (otherSpell != null)
        {
            SpellState state = spell.GetState(this);
            if (state.isPure && spell.Conjurer != otherSpell.Conjurer)
            {
                otherSpell.Reflect(spell.Conjurer);
            } else if (!state.isPure)
            {
                otherSpell.Reflect(GameControllerScript.GetPlayerByOther(otherSpell.Conjurer));
            }

            state.durability--;
            if (state.durability <= 0)
            {
                spell.Hit();
            }
        }
    }

    public void setPure(bool value)
    {
        isPure = value;
    }
}
