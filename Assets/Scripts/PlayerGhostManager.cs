using System.Collections;
using UnityEngine;

public class PlayerGhostManager : MonoBehaviour
{
    [SerializeField] GameObject playerGhost;
    [SerializeField] GameObject playerSprite;
    
    public void SpawnGhost(float duration, float interval)
    {
        StartCoroutine(Spawn(duration, interval));
    }

    IEnumerator Spawn(float duration, float interval)
    {
        float runtime = 0f;
        while (runtime < duration)
        {
            GameObject ghost = Instantiate(playerGhost, transform);
            ghost.GetComponent<SpriteRenderer>().sprite = playerSprite.GetComponent<SpriteRenderer>().sprite;
            ghost.transform.localScale = new Vector3(playerSprite.transform.localScale.x * -1, playerSprite.transform.localScale.y, playerSprite.transform.localScale.z);
            yield return new WaitForSeconds(interval);
            runtime += interval;
        }
    }
}
