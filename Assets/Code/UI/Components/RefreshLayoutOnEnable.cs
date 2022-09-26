using UI.Extensions;
using UnityEngine;

namespace UI.Components
{
    [RequireComponent(typeof(RectTransform))]
    public class RefreshLayoutOnEnable : MonoBehaviour
    {
        //private void Start() => (transform as RectTransform).RefreshContentFitter();

        private void OnEnable() => (transform as RectTransform).RefreshContentFitter();
    }
}