using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : MonoBehaviour
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Define.Scene type, Transform parents = null)
    {
        switch (CurrentScene.SceneType)
        {
            case Define.Scene.TitleScene:
                SceneChangeAnimation_In anim1 = Managers.Resource.Instantiate("SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_In>();
                anim1.transform.SetParent(parents);

                Time.timeScale = 1;

                StartCoroutine(LoadSceneAsync(GetSceneName(type)));

                // anim1.SetInfo(type, () =>
                // {
                //     Managers.Resource.Destroy(GameObject.Find("@UI_Root"));
                //     Managers.Clear();
                //     SceneManager.LoadScene(GetSceneName(type));
                // });
                break;
            case Define.Scene.GameScene:
                SceneChangeAnimation_In anim2 = Managers.Resource.Instantiate("SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_In>();
                anim2.transform.SetParent(parents);

                Time.timeScale = 1;

                StartCoroutine(LoadSceneAsync(GetSceneName(type)));

                // anim2.SetInfo(type, () =>
                // {
                //     Managers.Resource.Destroy(Managers.UI.SceneUI.gameObject);
                //     Managers.Clear();
                //     SceneManager.LoadScene(GetSceneName(type));
                // });
                break;
            case Define.Scene.LobbyScene:
                SceneChangeAnimation_In anim3 = Managers.Resource.Instantiate("SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_In>();
                anim3.transform.SetParent(parents);

                Time.timeScale = 1;

                StartCoroutine(LoadSceneAsync(GetSceneName(type)));

                // anim3.SetInfo(type, () =>
                // {
                //     Managers.Resource.Destroy(Managers.UI.SceneUI.gameObject);
                //     Managers.Clear();
                //     SceneManager.LoadScene(GetSceneName(type));
                // });
                break;
        }

    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        // Managers.Resource.Destroy(GameObject.Find("@UI_Root"));
        // Managers.Clear();
    }

}
