using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.Components.Buttons
{
    /// <summary>
    /// Inherit from this class to add click functionality via scripts by overwriting the abstract 'OnClick' function.
    /// </summary>

    public abstract class AbstractButton : TooltipRequester, IPointerClickHandler
    {
        private readonly UnityEvent onClick = new UnityEvent();

        protected override void Awake()
        {
            base.Awake();
            onClick.AddListener(OnClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onClick.RemoveListener(OnClick);
        }

        protected abstract void OnClick();

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable)
                return;

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            onClick.Invoke();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (interactable)
                PlayHoverSound();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (interactable)
                PlayClickSound();
        }

        public virtual void PlayHoverSound() { } // => AudioProvider.Instance.PlayButtonHover();
        public virtual void PlayClickSound() { } // => AudioProvider.Instance.PlayButtonClick();
    }
}