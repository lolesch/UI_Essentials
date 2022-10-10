using UI.Extensions;
using UnityEngine;

namespace UI.Components.Buttons
{
    public class HyperlinkButton : AbstractButton
    {
        [SerializeField] private string linkToOpen;

        protected override void OnClick()
        {
            if (Application.isPlaying)
                Application.OpenURL(linkToOpen);

            Debug.Log($"OPEN URL:\t{linkToOpen.Colored(UIExtensions.LightBlue)}");
        }
    }
}