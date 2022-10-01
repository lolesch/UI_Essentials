using UnityEngine;

namespace UI.Components.Buttons
{
    public class HyperlinkButton : AbstractButton
    {
        [SerializeField] private string linkToOpen;

        protected override void OnClick() => Application.OpenURL(linkToOpen);
    }
}