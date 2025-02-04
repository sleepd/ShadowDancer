using UnityEngine;

public class PlayerSpriteHelper : MonoBehaviour
{
    private PlayerController _playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerController = transform.parent.GetComponent<PlayerController>();
        
    }


    public void OnLandingAnimationEnd()
    {
        _playerController.OnLandingAnimationEnd();
    }
}
