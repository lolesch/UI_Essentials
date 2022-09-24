using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Base
{
    public abstract class AbstractToggle : TooltipRequester, IPointerClickHandler
    {
        [SerializeField] protected bool isEnabledAtStrat = false;
        public bool IsOn { get; private set; } = false;

        public RadioGroup radioGroup = null;

        public RadioGroup RadioGroup => radioGroup != null ? radioGroup : radioGroup = GetComponentInParent<RadioGroup>();

        protected override void Awake()
        {
            base.Awake();

            if (RadioGroup == null)
                Debug.LogWarning(new MissingComponentException($"Missing RadioGroup on {name} under {transform.parent.name}"));

            SetToggle(isEnabledAtStrat);
        }

        protected virtual void SetToggle(bool isOn)
        {
            IsOn = isOn;

            // TODO catch DOTween errors
            transform.DOScale(IsOn ? 1.12f : 1, .2f).SetEase(Ease.InOutSine);

            DoStateTransition(IsOn ? SelectionState.Selected : SelectionState.Normal, true);

            NotifyGroup(IsOn);
        }

        public void Toggle() => SetToggle(!IsOn);
        public void Toggle(bool isOn) => SetToggle(isOn);

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable)
                return;

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (RadioGroup && !RadioGroup.AllowSwitchOff)
                if (IsOn)
                    return;

            SetToggle(!IsOn);
        }

        void NotifyGroup(bool isOn)
        {
            if (!RadioGroup)
                return;

            if (isOn)
                RadioGroup.SetOtherTogglesOff(this);
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

            if (interactable)
                if (!IsOn)
                    PlayClickSound();
            //AudioProvider.Instance.PlayToggle();
        }

        public virtual void PlayHoverSound() { } // => AudioProvider.Instance.PlayButtonHover();
        public virtual void PlayClickSound() { } // => AudioProvider.Instance.PlayButtonClick();
    }
}