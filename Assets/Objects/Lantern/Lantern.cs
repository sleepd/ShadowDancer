using UnityEngine;
using UnityEngine.VFX;

public class Lantern : MonoBehaviour
{
    private Animator _animator;
    private GameObject _sprite;
    private VisualEffect _visualEffect;
    [SerializeField] private AudioSource sfx_burst;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject targetObject = GameObject.FindWithTag("VFX Lantern Broken");

        if (targetObject != null)
        {
            _visualEffect = targetObject.GetComponent<VisualEffect>();
        }
        else
        {
            Debug.Log("Object with tag not found.");
        }

        _sprite = transform.Find("Sprite").gameObject;
        _animator = _sprite.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger("Burst");
            sfx_burst.Play();
        }
        _visualEffect.SetVector3("BurstPosition", transform.position);
        _visualEffect.SendEvent("LanternBurst");

    }

    public void OnBurstEnd()
    {
        Destroy(gameObject);
    }
}

internal class VAudioClip
{
}