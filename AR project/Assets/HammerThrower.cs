using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerThrower : MonoBehaviour
{
    public GameObject hammerPrefab;
    public GameObject target;
    public GameObject explosionPrefab;

    public StateManager stateManager;
    public bool isTracking;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void ThrowHammer()
    {
        GameObject hammer = Instantiate(hammerPrefab, this.transform.position, this.transform.rotation);
        StartCoroutine(MoveTowardsTarget(hammer));
    }

    private IEnumerator MoveTowardsTarget(GameObject hammer)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition;

        endPosition = target.transform.position;
        endPosition.y -= 0f; // Adjust this value to set how much below the image we want the grenade to land.   

        Quaternion startRotation = this.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, 90);

        Vector3 hvel = Vector3.Scale((endPosition - startPosition), new Vector3(0.5f, 0.5f, 0.5f)); // grenade arrives in 2s, travels half the vector in 1s
        float vvel = 2.0f;

        float journeyDuration = 3f;
        float startTime = Time.time;

        while ((Time.time - startTime) < journeyDuration)
        {
            if (!hammer)
            {
                break;
            }
            float x = (Time.time - startTime) / journeyDuration;

            // drag and gravity
            vvel *= 0.99f;
            vvel -= 0.05f;

            hammer.transform.position += hvel * Time.deltaTime;
            hammer.transform.position += new Vector3(0f, vvel * Time.deltaTime, 0f);
            hammer.transform.rotation = Quaternion.Lerp(startRotation, endRotation, x);

            yield return null;
        }

        //Destroy(grenadePrefab);
        Vector3 pos = stateManager.isTracking ? target.transform.position : hammer.transform.position;
        Instantiate(explosionPrefab, pos, transform.rotation);
    }
}
