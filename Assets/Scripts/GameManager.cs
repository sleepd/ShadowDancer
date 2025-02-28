using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
