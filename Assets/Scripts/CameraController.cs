using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] bool lookAhead = true;
    [SerializeField] Vector2 screenOffset = Vector2.zero;
    [SerializeField] Vector2 aheadDistance;
    [SerializeField] Vector2 moveSpeed;
    [SerializeField] float zDistance = 100f;

    [Header("Confiner")]
    [SerializeField] BoxCollider2D boundingBox;

    Vector2 _targetPosition = new();
    Vector2 _currentOffset = new();
    float _horizExtent = 20;
    float _vertExtent = 11.25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = GetBasePosition();
    }

    // Update is called once per frame
    void Update()
    {
            Vector2 basePosition = GetBasePosition();
            Vector2 currentPosition = basePosition + _currentOffset;
            // calculate x axis offset
            _targetPosition.x = basePosition.x + aheadDistance.x * player.faceing;

            // calulate y axis offset
            if (!player.onGround) _targetPosition.y = basePosition.y - aheadDistance.y;
            // else if (player.velocity.y < 0) _targetPosition.y = basePosition.y - aheadDistance;
            else _targetPosition.y = basePosition.y;

            if (Vector2.Distance(currentPosition, _targetPosition) > 0.1f)
            {
                _currentOffset += moveSpeed * Time.deltaTime * (_targetPosition - currentPosition).normalized;
            }
            Vector3 position = basePosition + _currentOffset;
            // check the bounding box
            if (boundingBox != null)
            {
                

                // 计算考虑相机视野后的边界
                float minX = boundingBox.bounds.min.x + _horizExtent;
                float maxX = boundingBox.bounds.max.x - _horizExtent;
                float minY = boundingBox.bounds.min.y + _vertExtent;
                float maxY = boundingBox.bounds.max.y - _vertExtent;
                position.x = Mathf.Clamp(position.x, minX, maxX);
                position.y = Mathf.Clamp(position.y, minY, maxY);
            }
            
            position.z = zDistance;
            transform.position = position;            
    }

    Vector2 GetBasePosition()
    {
        Vector3 pos = player.transform.position;
        pos.x += screenOffset.x;
        pos.y += screenOffset.y;
        return pos;
        
    }
}
