using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    public Animator FadeAnimator;
    [Button("StartGame")]
    public void StartGame()
    {
        StartCoroutine(FadeAndStart());
    }

    private IEnumerator FadeAndStart()
    {
        FadeAnimator.SetTrigger("Fade");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Arena");
    }
}