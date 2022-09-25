using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    [RequireComponent(typeof(Canvas), typeof(CanvasScaler))]
    public class RootCanvas : MonoBehaviour
    {
        [SerializeField] private bool isPersistant = false;

        [SerializeField] private Canvas canvas;
        public Canvas Canvas => canvas != null ? canvas : canvas = GetComponent<Canvas>();

        [SerializeField] private CanvasScaler scaler;
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
