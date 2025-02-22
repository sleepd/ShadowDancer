using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
    private Vector3 _position;
    [SerializeField] float duration = 1f;
    float lifetime;

    void OnEnable()
    {
        _position = transform.position;
        lifetime = duration;
    }

    // Update is called once per frame
    void Update()
    {
        // stay at the same place
        transform.position = _position;
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

}
