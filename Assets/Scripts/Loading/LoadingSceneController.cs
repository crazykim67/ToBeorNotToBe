using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    private static string nextScene;

    [SerializeField]
    private Image loadingBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    public IEnumerator LoadSceneAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;

        while (!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                loadingBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.deltaTime;
                loadingBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(loadingBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
