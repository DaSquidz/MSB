using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameController : MonoBehaviour
{
    public Image fadeImage;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI resultText2;
    public float delayBeforeFade = 0.1f;
    public float fadeDuration = 3f;
    public float displayDuration = 5f;

    void Start()
    {
        fadeImage.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
        resultText2.gameObject.SetActive(false);
    }

    public void EndGame(bool playerWon)
    {
        resultText.text = playerWon ? "You Win!" : "You Lose!";
        StartCoroutine(EndGameSequence());
    }

    IEnumerator EndGameSequence()
    {
        yield return new WaitForSeconds(delayBeforeFade);
        
        fadeImage.gameObject.SetActive(true);
        resultText.gameObject.SetActive(true);
        resultText2.gameObject.SetActive(true);
        
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, normalizedTime);
            yield return null;
        }
        fadeImage.color = Color.white;
        
        yield return new WaitForSeconds(displayDuration);
        
        SceneManager.LoadScene("MainMenu");
    }
}