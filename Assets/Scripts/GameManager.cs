using UnityEngine;

public class GameManager : MonoBehaviour
{
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
}
