using DG.Tweening;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Displays
{
    [RequireComponent(typeof(CanvasGroup), typeof(Canvas), typeof(GraphicRaycaster))]
    public abstract class AbstractDisplay : MonoBehaviour
    {
        [field: SerializeField, Range(0, 1)] public float FadeDuration { get; private set; } = .2f;

        protected CanvasGroup CanvasGroup { get; private set; } = null;

        protected Canvas RootCanvas { get; private set; } = null;

        protected virtual void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();

            if (CanvasGroup == null)
                Debug.LogError(new MissingComponentException($"Missing CanvasGroup on {name} under {transform.parent.name}"));

            RootCanvas = transform.root.GetComponent<Canvas>();

            if (RootCanvas == null)
                RootCanvas = transform.root.GetComponentInChildren<Canvas>();

            if (RootCanvas == null)
                Debug.LogError(new MissingComponentException($"Missing Canvas on {name} under {transform.parent.name}"));
            else if (RootCanvas.renderMode == RenderMode.WorldSpace)
                RootCanvas.worldCamera = Camera.main;

            FadeOut(0);
        }

        protected virtual void OnDestroy()
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
