using UI.Extensions;
using UnityEngine;

namespace UI.Components.Buttons
{
    public class QuitGameButton : AbstractButton
    {
        protected override void OnClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
            Debug.LogWarning("Quitting the game".Colored(Color.red));
        }
    }
}
