using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    private Rigidbody rb;
    public Orbit gravity { get; set; }

    [SerializeField]
    private float rotationSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gravity)
        {
            Vector3 gravityUp;
            if (gravity.cilinder)
            {
                gravityUp = new Vector3(1, transform.position.y - gravity.transform.position.y, transform.position.z - gravity.transform.position.z).normalized;
            }
            else
            {
                if (gravity.fixedDirection)
                {
                    gravityUp = gravity.transform.up;
                }
                else
                {
                    gravityUp = (transform.position - gravity.transform.position).normalized;
                }
            }
            //Quaternion targetRotation = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;
            //transform.up = Vector3.Lerp(transform.up, gravityUp, rotationSpeed * Time.deltaTime);
            rb.AddForce((-gravityUp * gravity.gravity) * rb.mass);
        }
    }
}
