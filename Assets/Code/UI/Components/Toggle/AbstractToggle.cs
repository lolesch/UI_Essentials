using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Components.Toggle
{
    [RequireComponent(typeof(Canvas))]
    public abstract class AbstractToggle : TooltipRequester, IPointerClickHandler
    {
        [Header("ToggleSettings")]

        // test wich one to use or if both are necessary
        [SerializeField] protected bool isToggledOnAwake = false;
        [SerializeField] protected bool isToggledOnEnable = false;

        [SerializeField] protected bool doScaleOnToggle = false;

        [SerializeField] private Sprite toggledOffSprite;
        [SerializeField] private Sprite toggledOnSprite;

        [field: SerializeField, ReadOnly] public bool IsOn { get; private set; } = false;

        [SerializeField, ReadOnly] protected RadioGroup radioGroup = null;
        public RadioGroup RadioGroup => radioGroup != null ? radioGroup : radioGroup = GetComponentInParent<RadioGroup>();

        protected override void OnValidate()
        {
            base.OnValidate();

            IsOn = isToggledOnAwake || isToggledOnEnable;

            NotifyGroup(IsOn);

            if (RadioGroup != null && RadioGroup.transform != transform.parent)
                radioGroup = null;
        }

        protected override void Awake()
        {
            base.Awake();

            SetToggle(isToggledOnAwake);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!isToggledOnAwake)
                SetToggle(isToggledOnEnable);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (DOTween.IsTweening(targetGraphic.transform))
                DOTween.Kill(targetGraphic.transform);
        }

        //public void Toggle() => SetToggle(!IsOn);

        public virtual void SetToggle(bool isOn)
        {
            IsOn = isOn;

            if (toggledOffSprite != null && toggledOnSprite != null)
                (targetGraphic as Image).sprite = isOn ? toggledOnSprite : toggledOffSprite;

            if (doScaleOnToggle)
                targetGraphic.transform.DOScale(IsOn ? 1.12f : 1, .2f).SetEase(Ease.InOutSine);

            DoStateTransition(IsOn ? SelectionState.Selected : SelectionState.Normal, true);

            PlayToggleSound(isOn);

            NotifyGroup(IsOn);
        }

        protected void NotifyGroup(bool isOn)
        {
            if (!RadioGroup)
                return;

            if (isOn)
                RadioGroup.SetOtherTogglesOff(this);
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