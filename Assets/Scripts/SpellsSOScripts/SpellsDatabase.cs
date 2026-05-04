using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SpellsDatabase
{
    private static Dictionary<FixedBuffer<Keys>, BaseSpellSO> allBaseSpells;
    private static Dictionary<FixedSet<SpellSO>, FusionSpellSO> allFusionSpells;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize() { 
        allBaseSpells = new Dictionary<FixedBuffer<Keys>, BaseSpellSO>();
        BaseSpellSO[] baseSpells = Resources.LoadAll<BaseSpellSO>("BaseSpells"); 
        foreach (var data in baseSpells) {
            allBaseSpells[new FixedBuffer<Keys>(4, data.Combo)] = data;
        }
        allFusionSpells = new Dictionary<FixedSet<SpellSO>, FusionSpellSO>();
        FusionSpellSO[] fusionSpells = Resources.LoadAll<FusionSpellSO>("FusionSpells");
        foreach (var data in fusionSpells)
        {
            allFusionSpells[new FixedSet<SpellSO>(4, data.Components)] = data;
        }

    }
    public static BaseSpellSO GetBaseSpellByCombo(FixedBuffer<Keys> combo) {
        allBaseSpells.TryGetValue(combo, out BaseSpellSO spell);
        return spell;
    }

    public static FusionSpellSO getFusionSpellByComponents(FixedSet<SpellSO> components)
    {
        var array = components.ToArray();

        // tenta todas as permutações possíveis
        foreach (var perm in GetPermutations(array))
        {
            var key = new FixedSet<SpellSO>(4, perm);

            if (allFusionSpells.TryGetValue(key, out FusionSpellSO spell))
            {
                return spell;
            }
        }

        return null;
    }

    private static IEnumerable<SpellSO[]> GetPermutations(SpellSO[] list)
    {
        return Permute(list, 0);
    }

    private static IEnumerable<SpellSO[]> Permute(SpellSO[] list, int index)
    {
        if (index >= list.Length)
        {
            yield return list.ToArray();
            yield break;
        }

        for (int i = index; i < list.Length; i++)
        {
            Swap(list, index, i);

            foreach (var perm in Permute(list, index + 1))
                yield return perm;

            Swap(list, index, i);
        }
    }

    private static void Swap(SpellSO[] arr, int a, int b)
    {
        (arr[a], arr[b]) = (arr[b], arr[a]);
    }
}
