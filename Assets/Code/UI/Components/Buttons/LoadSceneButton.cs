using UI.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Components.Buttons
{
    public class LoadSceneButton : AbstractButton
    {
        [Space]
        [SerializeField, SceneRef] private string sceneToLoad;

        protected override void OnClick()
        {
            if (sceneToLoad != "")
                SceneManager.LoadSceneAsync(sceneToLoad);

            Debug.LogWarning($"{"SCENE:".Colored(Color.red)}\t{sceneToLoad.Colored(UIExtensions.LightBlue)}");
        }
    }
}
