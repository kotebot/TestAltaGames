using System.Collections;
using UnityEngine;

namespace Ostryzhnyi.QuickRollGame.Scripts.Blockers.Impl
{
    [RequireComponent(typeof(MeshRenderer))]
    public class BlickAnimation : MonoBehaviour
    {
        [SerializeField] private Color _toColor;
        [SerializeField] private float _duration = 1.0f;

        private Color _baseColor = Color.clear;
        private Coroutine _coroutine;
        private Material _material;

        private void Awake()
        {
            _material = GetComponent<MeshRenderer>().materials[0];

            _baseColor = _material.color;
        }

        private void OnEnable()
        {
            _coroutine = StartCoroutine(ChangeColor());
        }

        private void OnDisable()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
        }

        private IEnumerator ChangeColor()
        {
            while (isActiveAndEnabled)
            {
                yield return StartCoroutine(LerpColor(_baseColor, _toColor));

                yield return StartCoroutine(LerpColor(_toColor, _baseColor));
            }
        }

        private IEnumerator LerpColor(Color fromColor, Color toColor)
        {
            var elapsedTime = 0f;

            while (elapsedTime < _duration)
            {
                _material.color = Color.Lerp(fromColor, toColor, elapsedTime / _duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _material.color = toColor;
        }
    }
}