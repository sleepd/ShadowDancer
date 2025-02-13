using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float _moveRatio, startpos;
    private Camera _camera;   
    private float _width = 0;

    void Start()
    {
        _camera = Camera.main;
        startpos = transform.position.x;
        _width = GetComponent<SpriteRenderer>().bounds.size.x;
    }


    void Update()
    {
        float temp = (_camera.transform.position.x * (1 - _moveRatio));
        float dist = (_camera.transform.position.x * _moveRatio);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + _width) startpos += _width;
        else if (temp < startpos - _width) startpos -= _width;
    }
}
