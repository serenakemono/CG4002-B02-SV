using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public GameObject opponent;

    [Header("Settings")]
    public int totalGrenade;
    public float throwCooldown;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;
    float speed = 1;

    bool readyToThrow;

    public void Throw()
    {
        if (!readyToThrow || totalGrenade <= 0)
        {
            Debug.Log("Cannot throw");
            return;
        }

        readyToThrow = false;

        // instantiate object to throw
        GameObject grenade = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // set the initial speed
        grenade.GetComponent<Rigidbody>().velocity = speed * attackPoint.forward;

        // add force
        //Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;

        //projectileRb.AddForce(forceToAdd, ForceMode.VelocityChange);

        //totalGrenade--;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    void RotateThrowAngle()
    {
        float? angle = CalculateAngle(false);
        if (angle != null)
        {
            attackPoint.localEulerAngles = new Vector3(360f - (float)angle, 0f, 0f);
            //Debug.Log("New angle: " + attackPoint.localEulerAngles);
        }
    }

    float? CalculateAngle(bool low)
    {
        Vector3 targetDir = opponent.transform.position - attackPoint.transform.position;
        float y = targetDir.y;
        targetDir.y = 0f;
        float x = targetDir.magnitude - 1;
        float gravity = 9.8f;
        float sSqr = speed * speed;
        float underTheSqrRoot = (sSqr * sSqr) - gravity * (gravity * x * x + 2 * y * sSqr);

        if (underTheSqrRoot >= 0f)
        {
            float root = Mathf.Sqrt(underTheSqrRoot);
            float highAngle = sSqr + root;
            float lowAngle = sSqr - root;

            if (low)
                return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
            else
                return (Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg);
        }
        else
            return null;
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        readyToThrow = true;
    }

    //Update is called once per frame
    void Update()
    {
        RotateThrowAngle();
    }
}
