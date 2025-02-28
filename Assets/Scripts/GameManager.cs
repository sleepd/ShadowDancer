using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class GameManager : MonoBehaviour
{
    Image FadeImage;
    public bool isGamePaused { get => _isGamePaused;}
    private bool _isGamePaused = false;
    // singleton instance
    public static GameManager instance 
    {
        get 
        {
            return _instance;
        }
    }
    private static GameManager _instance;
    private GameObject _player;
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        QualitySettings.vSyncCount = 0;
    }
    
    private void Start()
    {
        FadeImage = GameObject.FindGameObjectWithTag("Fade Image").GetComponent<Image>();
        FadeIn();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeImage = GameObject.FindGameObjectWithTag("Fade Image").GetComponent<Image>();
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        _isGamePaused = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        _isGamePaused = false;
    }
    public void LoadNextScene(int index)
    {
        StartCoroutine(LoadNextSceneCoroutine(index));
    }

    private IEnumerator LoadNextSceneCoroutine(int index)
    {
        FadeOut();
        yield return new WaitForSeconds(1f); // 等待fade out完成
        SceneManager.LoadScene(index);
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCoroutine(1f, 0f, 1f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine(0f, 1f, 1f));
    }

    private IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color currentColor = FadeImage.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            FadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        // Ensure we end up exactly at the target alpha
        FadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, endAlpha);
    }

    private void OnDestroy()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
}
}
