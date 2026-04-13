using Assets.Scripts.Gameplay.Orbs;
using System;
using UnityEngine;

[Serializable]
public struct ElementWeight
{
    [field: SerializeField] public OrbPool Pool { get; private set; }
    [field: SerializeField] public float Weight { get; private set; }
}