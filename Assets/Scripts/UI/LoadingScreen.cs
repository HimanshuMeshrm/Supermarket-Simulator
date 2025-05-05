using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar;
    public TMP_Text progressText;

    void Start()
    {
        SaveManager.LoadGame();
        StartCoroutine(FakeLoadingProcess());
    }

    IEnumerator FakeLoadingProcess()
    {
        float targetProgress = 0f;
        float displayProgress = 0f;

        float duration = Random.Range(2.8f, 4f);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            targetProgress = Mathf.Clamp01(elapsed / duration);

          
            displayProgress = Mathf.Lerp(displayProgress, targetProgress, Time.deltaTime * 8f);
            progressBar.value = displayProgress;
            progressText.text = Mathf.FloorToInt(displayProgress * 100f) + "%";

            
            if (Random.value > 0.985f)
                yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

            yield return null;
        }

        progressBar.value = 1f;
        progressText.text = "100%";

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("MainMenu");
    }
}
