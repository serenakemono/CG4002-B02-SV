using UnityEngine;
using UnityEngine.UI;

public class BloodSplatter : MonoBehaviour
{
    public Image img;
    public bool isShowing;
    public float delay = 0.6f;
    float countdown;

    // Start is called before the first frame update
    void Start()
    {
        isShowing = false;
        img.enabled = false;
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShowing)
        {
            img.enabled = false;
        }

        else
        {
            img.enabled = true;
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                isShowing = false;
                countdown = delay;
            }
        }
    }

    public void SetIsShowing(bool status)
    {
        isShowing = status;
    }
}
