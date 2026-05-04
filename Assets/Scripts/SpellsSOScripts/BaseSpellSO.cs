using UnityEngine;

[CreateAssetMenu(fileName = "BaseSpell", menuName = "Spells/New Base Spell")]
public class BaseSpellSO : SpellSO
{
    [SerializeField]private Keys[] combo = new Keys[4];
    public Keys[] Combo => combo;

    /*
    public override void OnDie(SpellScript spell)
    {
        
    }

    public override void OnStart(SpellScript spell)
    {
        
    }

    public override void OnUpdate(SpellScript spell)
    {
        Vector3 move = spell.Direction.normalized * spell.XSpeed * Time.deltaTime;
        spell.transform.position += move;
    }

    */
}
