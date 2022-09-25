using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Base
{
    public class OpenHyperlinks : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private string linkToOpen;

        public void OnPointerClick(PointerEventData eventData) => Application.OpenURL(linkToOpen);

    }
}