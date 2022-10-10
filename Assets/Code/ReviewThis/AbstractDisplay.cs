using DG.Tweening;
using UI.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components.Displays
{
    [RequireComponent(typeof(CanvasGroup), typeof(GraphicRaycaster))]
    public abstract class AbstractDisplay : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] protected float fadeDuration = .2f;
        [SerializeField] protected Vector2 scaleTo = Vector2.one;
        [SerializeField] protected Vector2 moveTo = Vector2.zero;

        // TODO continue here (implement scale and move OnEnable and OnDisable
        protected bool IsScaling => 0 < fadeDuration;
        protected bool IsFading => Vector2.one != scaleTo;
        protected bool IsMoving => Vector2.zero != moveTo;


        [SerializeField, ReadOnly] protected CanvasGroup canvasGroup = null;
        public CanvasGroup CanvasGroup => canvasGroup != null ? canvasGroup : canvasGroup = GetComponentInParent<CanvasGroup>();

        protected virtual void Awake() => FadeOut(0);

        protected virtual void OnDestroy()
        {
            if (CanvasGroup != null)
                if (DOTween.IsTweening(CanvasGroup))
                    DOTween.Kill(CanvasGroup);
        }

        protected virtual void OnDisable()
        {
            if (CanvasGroup != null)
                if (DOTween.IsTweening(CanvasGroup))
                    DOTween.Kill(CanvasGroup);
        }

        public virtual Tween FadeIn(float fadeDuration)
        {
            if (CanvasGroup == null)
                return null;

            if (DOTween.IsTweening(CanvasGroup))
                DOTween.Kill(CanvasGroup);

            (transform as RectTransform).RefreshContentFitter();

            return CanvasGroup.DOFade(1, fadeDuration).SetEase(Ease.InQuad).OnComplete(() => CanvasGroup.blocksRaycasts = true);
        }

        public virtual Tween FadeOut(float fadeDuration)
        {
            if (CanvasGroup == null)
                return null;

            if (DOTween.IsTweening(CanvasGroup))
                DOTween.Kill(CanvasGroup);

            CanvasGroup.blocksRaycasts = false;

            return CanvasGroup.DOFade(0, fadeDuration).SetEase(Ease.InQuad);
        }
    }
}
