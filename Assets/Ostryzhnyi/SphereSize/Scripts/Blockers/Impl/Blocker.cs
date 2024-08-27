using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blockers.Api;
using Blockers.Data;
using UnityEngine;

namespace Blockers.Impl
{
    public class Blocker : MonoBehaviour, IBlocker
    {
        [SerializeField] private List<StateByObject> _states = new List<StateByObject>();
        [SerializeField] private LayerMask _mask;

        [SerializeField, Range(0.01f, 2f)]
        private float _delayBeforeChangeState = .1f;
        
        public void SetState(BlockerState state)
        {
            if(state == BlockerState.Removed)
            {
                Destroy(gameObject);
                return;
            }
            
            DisableAllStates();
            
            _states.FirstOrDefault(pair => pair.State == state)?.Object.SetActive(true);
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
                state.Object.SetActive(false);
            }
        }
    }

    [Serializable]
    public class StateByObject
    {
        public BlockerState State;
        public GameObject Object;

    }
}