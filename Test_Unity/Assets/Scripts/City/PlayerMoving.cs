using System;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeReference] Animator animator;
    [SerializeField] float Speed = 5;
    [SerializeField] float MaxSpeed = 20;
    [SerializeField] float StrafeSpeed = 10;
    [SerializeField] float SpeedJump = 10;
    [SerializeField] float BuffSpeed = 50;
    [SerializeField] Camera cam;
    BuffTimer timer;

    Rigidbody rb;
    bool left = false;
    bool right = false;
    float camRotation;
    bool _isPause;
    bool _isRun = true;
    CharacterValues characterValues;

    public static Action onChangeRoad;
    public static Action onStopRun;
    public static Action onStartRun;

    public bool IsRun
    {
        set => _isRun = value;
        get => _isRun;
    }

    public bool IsPaused
    {
        set
        {
            _isPause = value;
        }
        get => _isPause;
    }

    float CurrentSpeed
    {
        get
        {
            if (BuffType.none != currentBuff)
            {
                return BuffSpeed;
            }
            else
            {
                var temp = transform.position.z / 10;
                temp = (temp < MaxSpeed) ? temp : MaxSpeed;
                return Speed + temp;
            }
        }
    }


    float Distance => transform.position.z / 10;

    int oldValueChangeRoadType = 0;
    bool changeRoadType {
        get
        {
            var temp = (int)transform.position.z / (500 /** CurrentSpeed * 0.1*/);
            if (oldValueChangeRoadType < temp)
            {
                oldValueChangeRoadType = temp;
                return true;
            }
            return false;
        }
    }

    BuffType currentBuff = BuffType.none;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator.SetBool("Run", true);
        characterValues = FindObjectOfType<CharacterValues>();
        timer = GetComponent<BuffTimer>();
    }

    private void OnEnable()
    {
        onStopRun += StopRun;
        onStartRun += StartRun;
        BuffTimer.OnSpeedTimerStoped += ResetBuffType;
    }

    private void OnDisable()
    {
        onStopRun -= StopRun;
        onStartRun -= StartRun;
        BuffTimer.OnSpeedTimerStoped -= ResetBuffType;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (_isPause || !_isRun)
        {
            return;
        }
        MovingByTransform();
    }

    void MovingByTransform()
    {
        Vector3 newVelocity = new Vector3(0, 0, 0);

        if (left)
        {
            newVelocity.x = -StrafeSpeed;
            //newPosition = transform.position - transform.right * StrafeSpeed * Time.deltaTime;
        }
        else if (right)
        {
            newVelocity.x = StrafeSpeed;
            //newPosition = transform.position + transform.right * StrafeSpeed * Time.deltaTime;
        }

        newVelocity.z = CurrentSpeed;
        rb.velocity = newVelocity;
        //newPosition.z += CurrentSpeed * Time.deltaTime;

        var newPos = transform.position;
        newPos.x = Mathf.Clamp(newPos.x, -4.5f, 4.5f);
        transform.position = newPos;

        if (changeRoadType)
        {
            onChangeRoad?.Invoke();
        }

        characterValues.DisplayValues(CurrentSpeed, Distance);
    }

    public void Jump()
    {
        rb.AddForce(transform.up * SpeedJump, ForceMode.Impulse);

        animator.SetTrigger("Jump");
    }

    public void BurnIntoWall()
    {
        onStopRun?.Invoke();
        animator.SetTrigger("BurnIntoWall");
    }

    /// <summary>
    /// if run - variable is true, otherwise - false
    /// </summary>
    /// <param name="run"></param>
    public void RunOrIdle(bool run)
    {
        IsRun = run;
        animator.SetBool("Run", run);

        if (run)
        {
            onStartRun?.Invoke();
        }
        else
        {
            onStopRun?.Invoke();
        }
    }

    public void FallForward()
    {
        onStopRun?.Invoke();
        animator.SetTrigger("FallForward");
    }

    public void Restart()
    {
        onStartRun?.Invoke();
        animator.SetTrigger("Restart");
        _isRun = true;
        _isPause = false;
        transform.position = new Vector3(0, 0, 0);
    }

    public void StopRun()
    {
        _isRun = false;
        Vector3 newVelocity = new Vector3(0, 0, 0);
        newVelocity.z = 0;
        rb.velocity = newVelocity;
    }

    public void StartRun()
    {
        _isRun = true;
        Vector3 newVelocity = new Vector3(0, 0, 0);
        newVelocity.z = CurrentSpeed;
        rb.velocity = newVelocity;
    }

    public void SetSpeedBuff(BuffType type)
    {
        var value = type switch
        {
            BuffType.SpeedBuff_10 => 10,
            BuffType.SpeedBuff_30 => 30,
            BuffType.SpeedBuff_50 => 50,
            BuffType.SpeedNerf_10 => -10,
            BuffType.SpeedNerf_20 => -20,
            BuffType.SpeedNerf_30 => -30,
            _ => 0
        };
        
        BuffSpeed = value;
        currentBuff = type;

        timer.StartSpeed();
    }

    public void ResetBuffType()
    {
        currentBuff = BuffType.none;
    }
}
