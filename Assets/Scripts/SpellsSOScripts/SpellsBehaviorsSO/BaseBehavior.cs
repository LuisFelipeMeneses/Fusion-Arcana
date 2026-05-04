using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseBehavior", menuName = "Spells/Behavior/New Base Behavior")]
public class BaseBehavior : SpellBehavior
{
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
            player.TakeDamage(spell.SpellData.Damage);
            spell.Hit();
        }
    }
}
