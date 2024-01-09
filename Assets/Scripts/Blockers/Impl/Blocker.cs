using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blockers.Api;
using Blockers.Data;
using Player.Api;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blockers.Impl
{
    public class Blocker : SerializedMonoBehaviour, IBlocker
    {
        [SerializeField, FoldoutGroup("Setup", false)]
        private Dictionary<BlockerState, GameObject> _states = new Dictionary<BlockerState, GameObject>();
        [SerializeField, FoldoutGroup("Setup", false)]
        private LayerMask _mask;

        [SerializeField, BoxGroup("Settings"), Range(0.01f, 2f)]
        private float _delayBeforeChangeState = .1f;
        
        public void SetState(BlockerState state)
        {
            if(state == BlockerState.Removed)
            {
                Destroy(gameObject);
                return;
            }
            
            DisableAllStates();
            
            _states[state].SetActive(true);
        }

        public void Explosion(float radius)
        {
            StartCoroutine(ExplosionRoutine(radius));
        }

        private IEnumerator ExplosionRoutine(float radius)
        {
            SetState(BlockerState.Removing);

            var neighbors = GetNeighbors(radius);
            
            foreach (var blocker in neighbors)
            {
                blocker.SetState(BlockerState.Removing);
            }
            
            yield return new WaitForSeconds(_delayBeforeChangeState);

            SetState(BlockerState.Removed);

            foreach (var blocker in neighbors)
            {
                blocker.SetState(BlockerState.Removed);
            }
        }

        private IEnumerable<IBlocker> GetNeighbors(float radius)
        {
            var overlapSphere = Physics.OverlapSphere(transform.position, radius, _mask);
            return overlapSphere.Select(blocker => blocker.GetComponent<IBlocker>());
        }

        private void DisableAllStates()
        {
            foreach (var state in _states)
            {
                state.Value.SetActive(false);
            }
        }
    }
}