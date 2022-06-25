using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEvent : MonoBehaviour
{
    [Header("ћаксимальна€ скорость сдвига в сторону")]
    public float StrafSpeed = 5f;

    CharacterController controller;
    Vector2 startPos = new();
    Resolution resolution;
    bool isSwipe;
    bool isTouch;

    private void Start()
    {
        resolution = Screen.currentResolution;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
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
                    var swipeDistHorizontal = (touch.position.x - startPos.x) * -1;
                    var procent = swipeDistHorizontal / resolution.width;

                    Vector3? direction = null;
                    if (swipeDistHorizontal < 0)//Right
                    {
                        direction = new Vector3(0, 0, procent);
                        Debug.Log($"Right: {swipeDistHorizontal}; width: {resolution.width}: proc: {procent}");
                    }
                    else if(swipeDistHorizontal > 0)//Left
                    {
                        direction = new Vector3(0, 0, procent);
                        Debug.Log($"Left: {swipeDistHorizontal}; width: {resolution.width}: proc: {procent}");
                    }

                    if (direction is not null)
                    {
                        //if (controller.transform.position.z < 4.5f || controller.transform.position.z > -4.5f)
                        //{
                        //}
                        controller.Move((Vector3)direction * StrafSpeed * Time.deltaTime);
                    }

                    break;
                case TouchPhase.Ended:
                    isSwipe = false;
                    isTouch = false;
                    startPos = new();
                break;
            }
        }
        //foreach(Touch touch in Input.touches)
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //    if (Physics.Raycast(ray))
        //    {
        //        Instantiate(particle, transform.position, transform.rotation);
        //    }
        //}
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
}
