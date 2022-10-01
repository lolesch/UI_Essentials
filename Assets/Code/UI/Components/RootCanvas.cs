using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    [RequireComponent(typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler))]
    public class RootCanvas : MonoBehaviour
    {
        [SerializeField] protected bool isPersistant = false;
        [SerializeField] protected ScreenOrientation orientation = ScreenOrientation.AutoRotation;
        [SerializeField, ReadOnly] protected Canvas canvas;
        [SerializeField, ReadOnly] protected CanvasScaler scaler;

        public Canvas Canvas => canvas != null ? canvas : canvas = GetComponent<Canvas>();

        public CanvasScaler Scaler => scaler != null ? scaler : scaler = GetComponent<CanvasScaler>();

        private void OnEnable()
        {
            if (isPersistant && Application.isPlaying)
                DontDestroyOnLoad(this.gameObject);

            Canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            Scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            Scaler.referenceResolution = new Vector2(1920, 1080);
        }
    }
}
