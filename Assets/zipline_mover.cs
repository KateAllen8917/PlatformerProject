using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zipline_mover : MonoBehaviour
{
    public Transform goal;
    public Transform player;
    public SphereCollider end_trigger;
    public float travelTime = 2f; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            StartCoroutine(HandleZipline());
        }
    }
    private IEnumerator HandleZipline()
    {
        end_trigger.isTrigger = false;
        yield return StartCoroutine(MovePlayer());
        yield return new WaitForSeconds(5f);
        end_trigger.isTrigger = true;
    }
    private IEnumerator MovePlayer()
    {
        Vector3 startPos = player.position;
        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            player.position = Vector3.Lerp(startPos, goal.position, elapsed / travelTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.position = goal.position;
    }
}
