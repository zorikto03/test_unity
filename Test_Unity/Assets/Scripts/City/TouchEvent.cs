using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEvent : MonoBehaviour
{
    public static TouchEvent Instance { get; private set; }


    [Header("Максимальная скорость сдвига в сторону")]
    [SerializeField] private float StrafSpeed = 5f;

    [Header("Максимальный стрейф в сторону")]
    [SerializeField] private float LeftRightBorder = 4.5f;

    [Header("Максимальный угол наклона камеры")]
    [SerializeField] private float CamRotating = 30f;

    [Header("Скорость поворота камеры")]
    [SerializeField] private float CamRotatingSpeed = 15;

    [Header("Скорость возврата камеры в 0")]
    [SerializeField] private float ReturningCamRotatingSpeed = 10;

    [Header("Объект камеры")]
    [SerializeField] private Camera cam;

    [SerializeField] public bool isPaused;

    private float _eulerZ;

    Vector2 startPos = new();
    Resolution resolution;
    bool isSwipe;
    float turningSide;

    private void Start()
    {
        isPaused = false;
        resolution = Screen.currentResolution;
        turningSide = resolution.width / 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startPos = touch.position;
                        StartCoroutine(Jump());
                        break;
                    case TouchPhase.Stationary:
                    case TouchPhase.Moved:
                        isSwipe = true;
                        var swipeDistHorizontal = (touch.position.x - startPos.x);
                        if (Mathf.Abs(swipeDistHorizontal) > turningSide)
                        {
                            swipeDistHorizontal = turningSide * (Mathf.Abs(swipeDistHorizontal) / swipeDistHorizontal);
                        }
                        var procent = swipeDistHorizontal / turningSide;

                        var newPosition = transform.position;

                        newPosition.x += procent * StrafSpeed * Time.deltaTime;

                        newPosition.x = Mathf.Clamp(newPosition.x, -LeftRightBorder, LeftRightBorder);

                        transform.position = newPosition;

                        //cam rotation
                        _eulerZ += procent * Time.deltaTime * CamRotatingSpeed;

                        _eulerZ = Mathf.Clamp(_eulerZ, -CamRotating, CamRotating);

                        cam.transform.eulerAngles = new Vector3(27, 0, _eulerZ);

                        break;
                    case TouchPhase.Ended:
                        isSwipe = false;
                        startPos = new();

                    break;
                }
            }
            else
            {
                if (Mathf.Round(_eulerZ) > 0)
                {
                    _eulerZ -= ReturningCamRotatingSpeed * Time.deltaTime;

                }
                else if (Mathf.Round(_eulerZ) < 0)
                {
                    _eulerZ += ReturningCamRotatingSpeed * Time.deltaTime;
                }

                cam.transform.eulerAngles = new Vector3(27, 0, _eulerZ);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isSwipe && !isPaused)
        {
            //cam.transform.rotation
            Debug.Log($"CamRotating: {cam.transform.rotation.z}");
        }
    }

    IEnumerator Jump()
    {
        if (!isSwipe)
        {
            yield return new WaitForSeconds(0.05f);
            if (!isSwipe)
            {
                Debug.Log("Tap");

            }
            else
            {
                yield break;
            }
        }
        else
        {
            yield break;
        }
    }

    IEnumerator Left()
    {
        Debug.Log("Left");
        yield return new WaitForSeconds(0.05f);
    }
    IEnumerator Right()
    {
        Debug.Log("Right");
        yield return new WaitForSeconds(0.05f);
    }

    private void Awake()
    {
        Instance = this;
    }
}
