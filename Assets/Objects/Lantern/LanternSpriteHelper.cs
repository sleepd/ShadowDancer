using UnityEngine;

public class LanternSpriteHelper : MonoBehaviour
{
    private Lantern _lantern;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lantern = transform.parent.GetComponent<Lantern>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBurstEnd()
    {
        _lantern.OnBurstEnd();
    }
}
