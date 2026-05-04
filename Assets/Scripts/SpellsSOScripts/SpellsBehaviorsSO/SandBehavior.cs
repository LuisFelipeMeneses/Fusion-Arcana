using UnityEngine;

[CreateAssetMenu(fileName = "SandBehavior", menuName = "Spells/Behavior/New Sand Behavior")]
public class SandBehavior : SpellBehavior
{
    [SerializeField] private float slowPercent = 0.5f;
    [SerializeField] private SpellSO fireSpell;
    [SerializeField] private SpellSO GlassSpell;

    public override void OnStart(SpellScript spell)
    {
        SpellState state = spell.GetState(this);
        state.slowPercent = slowPercent;
    }

    public override void OnCollide(SpellScript spell, Collider2D other)
    {
        SpellScript otherSpell = other.GetComponent<SpellScript>();
        if (otherSpell != null)
        {
            if (otherSpell.SpellData == fireSpell)
            {
                PlayerScript player = otherSpell.Conjurer.GetComponent<PlayerScript>();
                GameObject newSpell = Instantiate(spell.gameObject, spell.transform.position, Quaternion.identity);
                SpellScript newSpellScript = newSpell.GetComponent<SpellScript>();
                newSpellScript.Instantiate(GlassSpell, spell.Direction, spell.Conjurer);
                newSpell.transform.position = spell.transform.position;
                if (newSpellScript.TryGetBehavior<ReflectionBehavior>(out var impure))
                {
                    impure.setPure(false);
                } else
                {
                    Debug.LogError("The new spell doesn't have a ReflectionBehavior");
                }
                otherSpell.Die();
                spell.Die();
            }

            otherSpell.ChangeSpeed(otherSpell.XSpeed * slowPercent);
        }
    }


}
