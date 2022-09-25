using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Components.Buttons
{
    public class HyperlinkButton : AbstractButton, IPointerClickHandler
    {
        [SerializeField] private string linkToOpen;

        protected override void OnClick() => Application.OpenURL(linkToOpen);
    }
}