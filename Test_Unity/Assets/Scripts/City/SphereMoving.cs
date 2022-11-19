using UnityEngine;

public class SphereMoving : MonoBehaviour
{
    [SerializeReference] Animator animator;
    [SerializeField] float Speed = 5;
    [SerializeField] float MaxSpeed = 20;
    [SerializeField] float StrafeSpeed = 10;
    [SerializeField] float CamRotSpeed = 10;
    [SerializeField] Camera cam;

    float _eulerZ;
    bool left = false;
    bool right = false;
    float camRotation;
    float currentSpeed
    {
        get
        {
            var temp = transform.position.z / 10;
            temp = (temp < MaxSpeed) ? temp : MaxSpeed;
            return Speed + temp;
        }
    }


    void Start()
    {
        animator.SetBool("Run", true);
    }

    void Update()
    {
        if (Input.GetKey("a"))
        {
            left = true;
        }
        else { left = false; }

        if (Input.GetKey("d"))
        {
            right = true;
        }
        else { right = false; }

        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
        }
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = transform.position ;

        if (left)
        {
            newPosition = transform.position - transform.right * StrafeSpeed * Time.deltaTime;
            _eulerZ -= StrafeSpeed * Time.deltaTime;
        }
        else if (right)
        {
            newPosition = transform.position + transform.right * StrafeSpeed * Time.deltaTime;
            _eulerZ += StrafeSpeed * Time.deltaTime;
        }
        else
        {
            if (Mathf.Round(_eulerZ) > 0)
            {
                _eulerZ -= CamRotSpeed * Time.deltaTime;

            }
            else if (Mathf.Round(_eulerZ) < 0)
            {
                _eulerZ += CamRotSpeed * Time.deltaTime;
            }
        }

        newPosition.z += currentSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, -4.5f, 4.5f);
        transform.position = newPosition;

        _eulerZ = Mathf.Clamp(_eulerZ, -30f, 30f);
        Debug.Log($"currentRot after clamp : {_eulerZ}");
        cam.transform.eulerAngles = new Vector3(27, 0, _eulerZ);
    }
}
