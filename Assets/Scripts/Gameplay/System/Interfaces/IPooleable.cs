using Assets.Scripts.Gameplay.GameSystem.Object_Pool;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Systems
{
    public interface IPoolable
    {
        void OnGetFromPool();
    }
}