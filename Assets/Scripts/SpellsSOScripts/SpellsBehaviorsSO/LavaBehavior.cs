using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LavaBehavior", menuName = "Spells/Behavior/New Lava Behavior")]
public class LavaBehavior : SpellBehavior
{
    [SerializeField] private float delay = 1f;
    [SerializeField] private float duration = 5f;

    public override void OnStart(SpellScript spell)
    {
        SpellState state = spell.GetState(this);
        state.delay = delay;
        state.duration = duration;
    }
    public override void OnCollide(SpellScript spell, Collider2D other)
    {
        SpellScript otherSpell = other.gameObject.GetComponent<SpellScript>();

        if (otherSpell != null && spell.SpellData.StrongWith.Contains(otherSpell.SpellData))
        {
            otherSpell.Die();
        }

        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
        if (player != null && spell.Conjurer != player.gameObject)
        {
            SpellState state = spell.GetState(this);
            player.TakeDamage(spell.SpellData.Damage, state.delay, state.duration);
            spell.Hit();
        }
    }
}
