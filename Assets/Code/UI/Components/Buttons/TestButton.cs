using UI.Extensions;
using UnityEngine;

namespace UI.Components.Buttons
{
    public class TestButton : AbstractButton
    {
        protected override void OnClick() => Debug.Log($"BUTTON:\t{name.ColoredComponent()} was {"clicked".Colored(UIExtensions.Orange)}", this);
    }
}
