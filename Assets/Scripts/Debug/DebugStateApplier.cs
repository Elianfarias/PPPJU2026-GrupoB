using Assets.Scripts.Data.Elemental;
using Assets.Scripts.Gameplay.System.Elemental;
using UnityEngine;

namespace Assets.Scripts.DebugScripts
{
    public class DebugStateApplier : MonoBehaviour
    {
        [SerializeField] private ElementalStateHandler handler;
        [SerializeField] private ElementalStateData wetState;
        [SerializeField] private KeyCode applyKey = KeyCode.M;

        private void Update()
        {
            if (Input.GetKeyDown(applyKey))
                handler.ApplyState(wetState, gameObject);
        }
    }
}