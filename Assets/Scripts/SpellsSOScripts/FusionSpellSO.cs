using UnityEngine;

[CreateAssetMenu(fileName = "FusionSpell", menuName = "Spells/New Fusion Spell")]
public class FusionSpellSO : SpellSO
{
    [SerializeField] protected SpellSO[] components = new SpellSO[2];
    public SpellSO[] Components => components;
}
