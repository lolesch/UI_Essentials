using UI.Extensions;
using UnityEngine;

namespace UI.Components.Toggle
{
    public class TestToggle : AbstractToggle
    {
        public override void SetToggle(bool isOn)
        {
            base.SetToggle(isOn);

            Debug.Log($"TOGGLE:\t{name.ColoredComponent()} was toggled {(isOn ? "on" : "off").Colored(UIExtensions.Orange)}", this);
        }
    }
}
