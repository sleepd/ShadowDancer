using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController player; // the player object
    [SerializeField] bool lookAhead = true; // if the camera should look ahead
    [SerializeField] Vector2 screenOffset = Vector2.zero; // the offset of the camera from the player
    [SerializeField] Vector2 aheadDistance; // the distance the camera should look ahead
    [SerializeField] float moveSpeed; // the following speed of the camera
    [SerializeField] float zDistance = 100f; // the distance of the camera from the player in the z axis
    [SerializeField] float snapThreshold = 0.5f; // the threshold for the camera to snap to the target position

    [Header("Confiner")]
    [SerializeField] BoxCollider2D boundingBox; // the bounding box of the camera

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
            if (!lookAhead)
            {
                _currentOffset = Vector2.zero;
                _targetPosition = basePosition;
            }
            else
            {
                Vector2 currentPosition = basePosition + _currentOffset;
                // calculate x axis offset
                _targetPosition.x = basePosition.x + aheadDistance.x * player.faceing;

                // calulate y axis offset
                if (!player.onGround) _targetPosition.y = basePosition.y - aheadDistance.y;
                else _targetPosition.y = basePosition.y;

                // calculate the cameraoffset
                if (Vector2.Distance(currentPosition, _targetPosition) > snapThreshold)
                {
                    _currentOffset = Vector2.Lerp(_currentOffset, _targetPosition - currentPosition, moveSpeed * Time.deltaTime);
                }
                else
                {
                    _currentOffset = _targetPosition - currentPosition;
                }
            }
            // calculate the new position
            Vector3 position = basePosition + _currentOffset;
            // check the bounding box
            if (boundingBox != null)
            {
                float minX = boundingBox.bounds.min.x + _horizExtent;
                float maxX = boundingBox.bounds.max.x - _horizExtent;
                float minY = boundingBox.bounds.min.y + _vertExtent;
                float maxY = boundingBox.bounds.max.y - _vertExtent;
                position.x = Mathf.Clamp(position.x, minX, maxX);
                position.y = Mathf.Clamp(position.y, minY, maxY);
            }
            
            // set the position
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
