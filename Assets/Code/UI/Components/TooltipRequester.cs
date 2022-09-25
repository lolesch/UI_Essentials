using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Components
{
    [RequireComponent(typeof(GraphicRaycaster))]
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

            Debug.LogWarning($"{name} in {transform.parent.name} has the following tooltip:\n{tooltip}", this);
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

            Debug.Log($"current selected object: {eventData.selectedObject}");
        }
    }
}