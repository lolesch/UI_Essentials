using System;
using System.Collections.Generic;
using System.Linq;
using UI.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components.Toggle
{
    public class RadioGroup : MonoBehaviour
    {
        public List<AbstractToggle> RadioToggle => GetComponentsInChildren<AbstractToggle>(false).Where(x => x.interactable).ToList();

        public AbstractToggle ActivatedToggle { get; private set; }
        public int ActiveIndex => RadioToggle.IndexOf(ActivatedToggle);
        [field: SerializeField] public bool AllowSwitchOff { get; private set; } = false;

        public event Action OnGroupChanged;

        private void OnValidate()
        {
            if (!TryGetComponent(out LayoutGroup _))
                UIExtensions.MissingComponent(nameof(LayoutGroup), gameObject);
        }

        public void SetOtherTogglesOff(AbstractToggle activatedToggle)
        {
            if (activatedToggle == null || ActivatedToggle == activatedToggle)
                return;

            ActivatedToggle = activatedToggle;

            ActivatedToggle.SetToggle(true);

            foreach (var i in RadioToggle.Where(x => x.IsOn).Where(x => x != ActivatedToggle))
                i.SetToggle(false);

            OnGroupChanged?.Invoke();
        }
    }
}