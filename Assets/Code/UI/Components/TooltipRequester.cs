using UI.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Components
{
    [RequireComponent(typeof(RectTransform), typeof(GraphicRaycaster))]
    public class TooltipRequester : Selectable
    {
        // TODO: read tooltip from LOCA
        [TextArea] public string tooltip = "";

        private static TooltipRequester currentTooltipRequester = null;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!targetGraphic)
                targetGraphic = GetComponent<Graphic>();

            if (targetGraphic)
                targetGraphic.raycastTarget = true;

            Debug.Log($"{"Tooltip:".Colored(UIExtensions.Orange)}\t{name}\t{(tooltip == "" ? "NONE" : tooltip).Colored(UIExtensions.Orange)}", this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (targetGraphic)
                targetGraphic.raycastTarget = false;

            currentTooltipRequester = null;

            // TooltipProvider.Instance.HideTooltip();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (tooltip != string.Empty)
                currentTooltipRequester = this;

            // TooltipProvider.Instance.ShowTooltipData(tooltip);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            currentTooltipRequester = null;

            // TooltipProvider.Instance.HideTooltip();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            Debug.Log($"{"Selection:".Colored(UIExtensions.Orange)}\t{eventData.selectedObject}", this);
        }
    }
}