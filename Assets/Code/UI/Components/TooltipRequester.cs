using UI.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Components
{
    [RequireComponent(typeof(GraphicRaycaster), typeof(CanvasRenderer))]
    public class TooltipRequester : Selectable
    {
        // TODO: import and update tooltip from LOCA
        [SerializeField, TextArea] private string tooltip = "";

        private static TooltipRequester currentTooltipRequester = null;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (targetGraphic == null)
                targetGraphic = GetComponent<Graphic>();

            if (targetGraphic)
                targetGraphic.raycastTarget = true;
            else
                UIExtensions.MissingComponent(nameof(Graphic), gameObject);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            currentTooltipRequester = null;

            // TooltipProvider.Instance.HideTooltip();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (tooltip != "")
            {
                currentTooltipRequester = this;

                // Debug.Log($"{"Tooltip:".Colored(UIExtensions.Orange)}\t{name}\t{(tooltip == "" ? "NONE" : tooltip).Colored(UIExtensions.Orange)}", this);
            }

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

            UIExtensions.Select(eventData.selectedObject);
        }
    }
}