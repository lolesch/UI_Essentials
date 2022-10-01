using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Components.Buttons
{
    public class LoadSceneButton : AbstractButton
    {
        [Space]
        [SerializeField, SceneRef] private string sceneToLoad;

        protected override void OnClick() => SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
