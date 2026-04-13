using Assets.Scripts.Gameplay.Systems;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Orbs.Spells
{
    public abstract class SpellBase : MonoBehaviour, IPoolable, ISpell
    {
        private PlayerSpellPool ownerPool;

        public void SetPool(PlayerSpellPool pool)
        {
            ownerPool = pool;
        }

        public virtual void OnGetFromPool() { }

        public abstract void Execute(Vector3 origin, Vector3 direction);

        protected void ReturnToPool()
        {
            ownerPool.ReturnSpell(this);
        }
    }
}