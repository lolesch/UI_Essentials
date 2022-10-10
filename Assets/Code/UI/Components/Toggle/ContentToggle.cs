using UI.Components.Displays;
using UnityEngine;

namespace UI.Components.Toggle
{
    public class ContentToggle : AbstractToggle
    {
        [SerializeField] protected AbstractDisplay content;

        public override void SetToggle(bool isOn)
        {
            base.SetToggle(isOn);

            if (content)
                if (isOn)
                    content.FadeIn(.2f);
                else
                    content.FadeOut(.2f);
        }
    }
}
