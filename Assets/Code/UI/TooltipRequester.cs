using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Base
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public class TooltipRequester : Selectable
    {
        // LOCA
        [TextArea] public string tooltip = "";
        [SerializeField] protected bool showTooltip = false;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!targetGraphic)
                targetGraphic = GetComponent<Graphic>();

            if (targetGraphic)
                targetGraphic.raycastTarget = true;

            //if (SettingsProvider.Instance.UseDebugLogTooltips)
            {
                if (showTooltip)
                    Debug.LogWarning($"{name} in {transform.parent.name} will show the following tooltip:\n{tooltip}", this);
                else
                    Debug.LogWarning($"{name} in {transform.parent.name} will NOT show the following tooltip:\n{tooltip}", this);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            //if (TooltipProvider.IsInstantiated)
            //    TooltipProvider.Instance.HideTooltip();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (showTooltip && tooltip != string.Empty)
            {
                if (targetGraphic)
                    targetGraphic.raycastTarget = showTooltip;

                //TooltipProvider.Instance.ShowTooltipData(tooltip);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            //TooltipProvider.Instance.HideTooltip();
        }
    }
}