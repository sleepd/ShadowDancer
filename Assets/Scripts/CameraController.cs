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

    Vector2 _targetPosition = new();
    Vector2 _currentOffset = new();
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
