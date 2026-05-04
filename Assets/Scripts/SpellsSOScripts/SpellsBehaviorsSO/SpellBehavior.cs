using System.Linq;
using UnityEngine;

public abstract class SpellBehavior : ScriptableObject
{
    public virtual void OnStart(SpellScript spell) { }
    public virtual void OnUpdate(SpellScript spell) 
    {
        if (spell.XSpeed == 0) return;

        Vector3 move = spell.Direction.normalized * spell.XSpeed * Time.deltaTime;
        spell.transform.position += move;
    }
    public virtual void OnCollide(SpellScript spell, Collider2D other) { }
    public virtual void OnDie(SpellScript spell) { }
}
