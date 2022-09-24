using Creation;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ScreenRotator : Monoton<ScreenRotator>
{
    private ScreenOrientation orientation;

    private void Update()
    {
        if (Screen.orientation != orientation)
        {
            orientation = Screen.orientation;

            if (Screen.width < Screen.height)
                Screen.orientation = ScreenOrientation.LandscapeRight;
            else
                Screen.orientation = ScreenOrientation.AutoRotation;
        }
    }
}
