using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    [RequireComponent(typeof(CanvasScaler), typeof(GraphicRaycaster))]
    public class RootCanvas : MonoBehaviour
    {
        [SerializeField, ReadOnly] protected Canvas canvas;
        [SerializeField, ReadOnly] protected CanvasScaler scaler;
        [Space]
        [SerializeField] protected bool isPersistant = false;
        [SerializeField] protected ScreenOrientation orientation = ScreenOrientation.AutoRotation;
        [SerializeField] protected Vector2 referenceResolution = new(1920, 1080);

        public Canvas Canvas => canvas != null ? canvas : canvas = GetComponent<Canvas>();

        public CanvasScaler Scaler => scaler != null ? scaler : scaler = GetComponent<CanvasScaler>();

        private void OnValidate()
        {
            Canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            Scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            Scaler.referenceResolution = referenceResolution;
        }

        private void OnEnable()
        {
            if (isPersistant && Application.isPlaying)
                DontDestroyOnLoad(gameObject);
        }
    }
}
