using UnityEngine;

public class ExitAera : MonoBehaviour
{
    public int nextSceneIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.LoadNextScene(nextSceneIndex);
        }
    }
}
