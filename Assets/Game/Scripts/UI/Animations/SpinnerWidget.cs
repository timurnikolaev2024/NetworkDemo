using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI
{
    public sealed class Spinner : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 180f;
        [SerializeField] private bool _rotateInNegativeDirection = true;

        private Tween _tween;

        void OnEnable()
        {
            float direction = _rotateInNegativeDirection ? -1f : 1f;

            _tween = transform
                .DORotate(new Vector3(0f, 0f, 360f * direction), 360f / _rotationSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }

        void OnDisable()
        {
            _tween?.Kill();
        }
    }
}