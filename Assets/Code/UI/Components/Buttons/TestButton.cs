using UI.Extensions;
using UnityEngine;

namespace UI.Components.Buttons
{
    public class TestButton : AbstractButton
    {
        protected override void OnClick() => Debug.Log($"{"Button:".Colored(UIExtensions.Orange)}\t{nameof(TestButton)} was {"clicked".Colored(UIExtensions.Orange)}", this);
    }
}
