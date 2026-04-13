using Assets.Scripts.Gameplay.GameSystem.Object_Pool;
using Assets.Scripts.Gameplay.Orbs.Spells;
using Assets.Scripts.Gameplay.Systems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerSpellPool : PoolBase
{
    [SerializeField] private string spellId;

    private static Dictionary<string, PlayerSpellPool> registry = new();
    [SerializeField] private GameObject spellPrefab;

    private void Awake()
    {
        registry[spellId] = this;
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public static PlayerSpellPool Get(string spellId)
    {
        registry.TryGetValue(spellId, out var pool);
        return pool;
    }

    public SpellBase GetSpell()
    {
        var spell = Get() as SpellBase;
        spell.SetPool(this);
        return spell;
    }

    public void ReturnSpell(SpellBase spell)
    {
        Return(spell);
    }

    protected override IPoolable CreateNew()
    {
        var obj = Instantiate(spellPrefab, transform);
        obj.SetActive(false);
        return obj.GetComponent<SpellBase>();
    }
}