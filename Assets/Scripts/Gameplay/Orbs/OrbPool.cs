using UnityEngine;
using Assets.Scripts.Gameplay.GameSystem.Object_Pool;
using Assets.Scripts.Gameplay.Systems.Interfaces;

namespace Assets.Scripts.Gameplay.Orbs
{
    public class OrbPool : PoolBase
    {
        [SerializeField] private GameObject orbPrefab;
        [SerializeField] private OrbSettingsSO orbSettings;

        public OrbElement Element()
        {
            return orbSettings.Element;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public OrbPickable GetOrb()
        {
            var orb = Get() as OrbPickable;
            orb.SetPool(this);
            return orb;
        }

        public void ReturnOrb(OrbPickable orb)
        {
            Return(orb);
        }

        protected override IPoolable CreateNew()
        {
            var obj = Instantiate(orbPrefab, transform);
            obj.SetActive(false);

            var pickable = obj.GetComponent<OrbPickable>();
            pickable.Setup(orbSettings);
            return pickable;
        }
    }
}