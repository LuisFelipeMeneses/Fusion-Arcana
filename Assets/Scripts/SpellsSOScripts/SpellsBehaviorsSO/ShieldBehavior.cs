using UnityEngine;

[CreateAssetMenu(fileName = "ShieldBehavior", menuName = "Spells/Behavior/New Shield Behavior")]
public class ShieldBehavior: SpellBehavior
{

    [SerializeField] private int durability = 1;

    public override void OnStart(SpellScript spell)
    {
        spell.transform.position = new Vector2(3 * -spell.Direction.x, 0.5f);
        SpellState state = spell.GetState(this);
        state.durability = durability;
    }

    public override void OnCollide(SpellScript spell, Collider2D other)
    {
        SpellScript otherSpell = other.GetComponent<SpellScript>();
        if (otherSpell != null && spell.Conjurer != otherSpell.Conjurer)
        {
            otherSpell.Hit();
            SpellState state = spell.GetState(this);
            state.durability--;
            if (state.durability <= 0)
            {
                spell.Hit();
            }
        }
    }

    public override void OnUpdate(SpellScript spell)
    {
        
    }
}
