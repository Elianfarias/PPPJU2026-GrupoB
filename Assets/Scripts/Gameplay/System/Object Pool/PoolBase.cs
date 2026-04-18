using Assets.Scripts.Gameplay.Systems.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gameplay.GameSystem.Object_Pool
{
    public abstract class PoolBase : MonoBehaviour
    {
        [SerializeField] private int defaultCapacity = 5;
        [SerializeField] private int maxSize = 30;
        protected Queue<IPoolable> available = new();

        public virtual void Initialize()
        {
            for (int i = 0; i < defaultCapacity; i++)
                available.Enqueue(CreateNew());
        }

        protected abstract IPoolable CreateNew();

        public IPoolable Get()
        {
            IPoolable item = available.Count > 0 ? available.Dequeue() : CreateNew();

            (item as MonoBehaviour).gameObject.SetActive(true);
            item.OnGetFromPool();
            return item;
        }

        public void Return(IPoolable item)
        {
            if (available.Count >= maxSize)
            {
                Destroy((item as MonoBehaviour).gameObject);
                return;
            }

            (item as MonoBehaviour).gameObject.SetActive(false);
            available.Enqueue(item);
        }
    }
}