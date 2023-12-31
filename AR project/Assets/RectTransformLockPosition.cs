using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformLockPosition : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 startPosition;

    protected void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (startPosition != null)
        {
            rectTransform.position = startPosition;
        }
    }
}
