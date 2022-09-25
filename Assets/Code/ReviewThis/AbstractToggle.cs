using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Components.Toggle
{
    public abstract class AbstractToggle : TooltipRequester, IPointerClickHandler
    {
        [SerializeField] protected bool isEnabledOnAwake = false;
        [SerializeField] protected bool isEnabledOnEnable = false;
        [SerializeField, ReadOnly] protected RadioGroup radioGroup = null;

        public bool IsOn { get; private set; } = false;
        public RadioGroup RadioGroup => radioGroup != null ? radioGroup : radioGroup = GetComponentInParent<RadioGroup>();

        protected override void Awake()
        {
            base.Awake();

            SetToggle(isEnabledOnAwake);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            SetToggle(isEnabledOnEnable);
        }

        //public void Toggle() => SetToggle(!IsOn);

        public virtual void SetToggle(bool isOn)
        {
            IsOn = isOn;

            // TODO catch DOTween errors
            transform.DOScale(IsOn ? 1.12f : 1, .2f).SetEase(Ease.InOutSine);

            DoStateTransition(IsOn ? SelectionState.Selected : SelectionState.Normal, true);

            PlayToggleSound(isOn);

            NotifyGroup(IsOn);

            void NotifyGroup(bool isOn)
            {
                if (!RadioGroup)
                    return;

                if (isOn)
                    RadioGroup.SetOtherTogglesOff(this);
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable)
                return;

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (RadioGroup && !RadioGroup.AllowSwitchOff && IsOn)
                return;

            SetToggle(!IsOn);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (interactable)
                DoStateTransition(IsOn ? SelectionState.Selected : SelectionState.Normal, false);
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

            if (interactable && !IsOn)
                PlayClickSound();
        }

        public virtual void PlayHoverSound() { } // => AudioProvider.Instance.PlayButtonHover();
        public virtual void PlayClickSound() { } // => AudioProvider.Instance.PlayButtonClick();
        public virtual void PlayToggleSound(bool isOn) { } // => AudioProvider.Instance.PlayButtonClick();
    }
}