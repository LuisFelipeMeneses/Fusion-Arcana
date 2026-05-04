using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]private GameObject spellPrefab;
    public static event Action<SpellSO[], int> OnSpellCast;
    public static event Action<Keys[], int> OnKeysPressed;
    public static event Action<float, int> OnPlayerDamaged;
    private Animator anim;
    private FixedBuffer<Keys> combo = new(4);
    private FixedSet<SpellSO> spellsInventory = new(4);
    private Vector3 handPosition;
    [SerializeField] private int maxLife = 100;
    [SerializeField] private int life = 100;
    [SerializeField] private float spellDelay = 0.5f;
    private Vector2 direction;
    private int playerNumber;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (playerNumber == 1)
        {
            direction = Vector2.right;
        } else if (playerNumber == 2)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            direction = Vector2.left;
        }
        
        handPosition = new Vector3(4 * direction.x, 1, 0);
    }

    void Update()
    {

    }

    private void Cast()
    {
        BaseSpellSO spell = SpellsDatabase.GetBaseSpellByCombo(combo);
        if (spell != null)
        {
            spellsInventory.Add(spell);
            combo.Erase();
            OnSpellCast?.Invoke(spellsInventory.ToArray(true), playerNumber);
            OnKeysPressed?.Invoke(combo.ToArray(), playerNumber);
            anim.SetTrigger("Cast");
        }
    }

    private void Conjure()
    {
        if (spellsInventory.Count > 0)
        {
            anim.SetTrigger("Conjure");
        }
    }

    private void Merge()
    {
        int maxSize = 4;

        for (int start = 0; start < spellsInventory.Count; start++)
        {
            for (int size = maxSize; size >= 2; size--)
            {
                if (start + size > spellsInventory.Count)
                    continue;

                // monta combinação
                FixedSet<SpellSO> components = new(4);

                for (int i = 0; i < size; i++)
                {
                    components.Add(spellsInventory.ToArray()[start + i]);
                }

                SpellSO result = SpellsDatabase.getFusionSpellByComponents(components);

                if (result != null)
                {
                    for (int i = 0; i < size; i++)
                    {
                        spellsInventory.Remove(start);
                    }
                    spellsInventory.Add(result);
                    OnSpellCast?.Invoke(spellsInventory.ToArray(true), playerNumber);
                    anim.SetTrigger("Merge");
                    return; // importante: evita múltiplas fusões no mesmo frame
                }
            }
        }

    }

    [UsedImplicitly]
    private void SpawnSpell()
    {
        SpellSO spellSO = spellsInventory.RemoveFirst();
        GameObject spell = Instantiate(spellPrefab, transform.position + handPosition, Quaternion.identity);
        spell.GetComponent<SpellScript>().Instantiate(spellSO, direction, gameObject);
        OnSpellCast?.Invoke(spellsInventory.ToArray(true), playerNumber);
    }

    public void Instantiate(int playerNumber)
    {
        this.playerNumber = playerNumber;
    }

    public void OnUp()
    {
        combo.Add(Keys.Up);
        anim.SetTrigger("Up");
        OnKeysPressed?.Invoke(combo.ToArray(), playerNumber);
    }
    public void OnDown()
    {
        combo.Add (Keys.Down);
        anim.SetTrigger("Down");
        OnKeysPressed?.Invoke(combo.ToArray(), playerNumber);
    }
    public void OnLeft()
    {
        combo.Add(Keys.Left);
        anim.SetTrigger("Left");
        OnKeysPressed?.Invoke(combo.ToArray(), playerNumber);
    }
    public void OnRight()
    {
        combo.Add(Keys.Right);
        anim.SetTrigger("Right");
        OnKeysPressed?.Invoke(combo.ToArray(), playerNumber);
    }
    public void OnCast()
    {
        Cast();
    }
    public void OnConjure()
    {
        Conjure();
    }
    public void OnMerge()
    {
        Merge();
    }

    [UsedImplicitly]
    private void Lock()
    {
        anim.SetBool("isLocked", true);
    }

    [UsedImplicitly]
    private void Unlock()
    {
        anim.SetBool("isLocked", false);
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        OnPlayerDamaged?.Invoke((float)life / maxLife, playerNumber);
    }

    public void TakeDamage(int damage, float delay, float duration)
    {
        StartCoroutine(DamageOverTime(damage, delay, duration));
    }

    IEnumerator DamageOverTime(int damage, float delay, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            TakeDamage(damage);
            yield return new WaitForSeconds(delay);
            time += delay;
        }
    }
}
