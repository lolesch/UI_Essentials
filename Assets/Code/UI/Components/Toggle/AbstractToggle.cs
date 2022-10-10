using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Components.Toggle
{
    public abstract class AbstractToggle : TooltipRequester, IPointerClickHandler
    {
        [Header("ToggleSettings")]

        // test wich one to use or if both are necessary
        [SerializeField] protected bool isToggledOnAwake = false;
        [SerializeField] protected bool isToggledOnEnable = false;

        [SerializeField] protected bool doScaleOnToggle = false;
        [SerializeField] protected bool doScaleOnHover = false;

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

            if (isToggledOnAwake)
                SetToggle(isToggledOnAwake);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!isToggledOnAwake)
                SetToggle(isToggledOnEnable);

            if (radioGroup && interactable)
                radioGroup.Register(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (targetGraphic && DOTween.IsTweening(targetGraphic.transform))
                DOTween.Kill(targetGraphic.transform);

            if (radioGroup)
                radioGroup.Unregister(this);
        }

        //public void Toggle() => SetToggle(!IsOn);

        public virtual void SetToggle(bool isOn)
        {
            IsOn = isOn;

            if (toggledOffSprite != null && toggledOnSprite != null)
                (targetGraphic as Image).sprite = IsOn ? toggledOnSprite : toggledOffSprite;

            if (targetGraphic && doScaleOnToggle)
                Scale(IsOn, 1.12f);

            DoStateTransition(IsOn ? SelectionState.Selected : SelectionState.Normal, true);

            PlayToggleSound(IsOn);

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

            if (targetGraphic)
            {
                if (doScaleOnToggle)
                    Scale(IsOn, 1.12f);
                else if (doScaleOnHover && !IsOn)
                    Scale(false, 1.06f);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (interactable)
                PlayHoverSound();

            if (targetGraphic && doScaleOnHover && !IsOn)
                Scale(true, 1.06f);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (interactable && !IsOn)
                PlayClickSound();
        }

        private void Scale(bool condition, float factor) => targetGraphic.transform.DOScale(condition ? factor : 1, .15f).SetEase(Ease.InOutSine);

        public virtual void PlayHoverSound() { } // => AudioProvider.Instance.PlayButtonHover();
        public virtual void PlayClickSound() { } // => AudioProvider.Instance.PlayButtonClick();
        public virtual void PlayToggleSound(bool isOn) { } // => AudioProvider.Instance.PlayButtonClick();
    }
}