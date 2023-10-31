using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrower : MonoBehaviour
{
    public GameObject spearPrefab;
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

    public void ThrowSpear()
    {
        GameObject spear = Instantiate(spearPrefab, this.transform.position, this.transform.rotation);
        StartCoroutine(MoveTowardsTarget(spear));
    }

    private IEnumerator MoveTowardsTarget(GameObject spear)
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
            if (!spear)
            {
                break;
            }
            float x = (Time.time - startTime) / journeyDuration;

            // drag and gravity
            vvel *= 0.99f;
            vvel -= 0.05f;

            spear.transform.position += hvel * Time.deltaTime;
            spear.transform.position += new Vector3(0f, vvel * Time.deltaTime, 0f);
            spear.transform.rotation = Quaternion.Lerp(startRotation, endRotation, x);

            yield return null;
        }

        //Destroy(grenadePrefab);
        Vector3 pos = stateManager.isTracking ? target.transform.position : spear.transform.position;
        Instantiate(explosionPrefab, pos, transform.rotation);
    }
}
