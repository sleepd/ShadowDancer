using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject TutorialPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TutorialPanel.SetActive(true);
            GameManager.instance.PauseGame();
        }
    }
}
