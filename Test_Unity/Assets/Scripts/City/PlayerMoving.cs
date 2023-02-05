using System;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] float StartSpeed = 5;
    [SerializeField] float MaxSpeed = 20;
    [SerializeField] float StrafeSpeed = 10;
    [SerializeField] float SpeedJump = 10;
    [SerializeField] float BrakeSpeed = 10;
    [SerializeField] Camera cam;
    
    public static Action onChangeRoad;
    public static Action onStopRun;
    public static Action onStartRun;

    BuffTimer timer;
    Rigidbody rb;
    bool left = false;
    bool right = false;
    bool accelerate = false;
    bool brake = false;
    float camRotation;
    bool _isPause;
    bool _isRun = true;
    bool _roadLeftRight = false;//indicator of road swaping to left or right
    CharacterValues characterValues;
    float _currentSpeed;
    float buffSpeed;
    float Distance => transform.position.z / 10;
    int oldValueChangeRoadType = 0;
    BuffType currentBuff = BuffType.none;


    public bool IsRun
    {
        set => _isRun = value;
        get => _isRun;
    }

    public bool IsPaused
    {
        set => _isPause = value;
        get => _isPause;
    }

    public float CurrentSpeed => _currentSpeed;
    public float GetMaxSpeed => MaxSpeed;

    public bool Accelerate => accelerate;
    public bool Brake => brake;
    public float BuffSpeed => buffSpeed;

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


    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        left = Input.GetKey(KeyCode.A);
        
        right = Input.GetKey(KeyCode.D);
        
        accelerate = Input.GetKey(KeyCode.W);

        brake = Input.GetKey(KeyCode.S);

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

    float SpeedAcceleration()
    {
        if (currentBuff == BuffType.none)
        {
            if (accelerate && !brake)
            {
                if (_currentSpeed < MaxSpeed)
                {
                    _currentSpeed += MaxSpeed / _currentSpeed * Time.deltaTime * 2;
                }
                else
                {
                    _currentSpeed -= MaxSpeed / _currentSpeed * Time.deltaTime;
                }
            }
            else if (!accelerate && !brake){
                if (_currentSpeed > StartSpeed)
                {
                    _currentSpeed -= MaxSpeed / 20 * Time.deltaTime;
                }
                else
                {
                    _currentSpeed = StartSpeed;
                }
            }
            else if (!accelerate && brake)
            {
                if (_currentSpeed > StartSpeed)
                {
                    _currentSpeed -= MaxSpeed / BrakeSpeed * Time.deltaTime;
                }
                else
                {
                    _currentSpeed = StartSpeed;
                }
            }
        }
        else
        {
            _currentSpeed += (MaxSpeed * buffSpeed) / _currentSpeed * Time.deltaTime * 2;
        }
        return _currentSpeed;
    }

    void MovingByTransform()
    {
        Vector3 newVelocity = LeftRight(new Vector3(0, 0, 0));

        newVelocity.z = SpeedAcceleration();
        rb.velocity = newVelocity;
        //newPosition.z += CurrentSpeed * Time.deltaTime;

        var newPos = transform.position;
        if (!_roadLeftRight)
        {
            newPos.x = Mathf.Clamp(newPos.x, -4.5f, 4.5f);
        }
        else
        {
            newPos.y = Mathf.Clamp(newPos.y, -4.5f, 4.5f);
        }
        transform.position = newPos;

        if (changeRoadType)
        {
            onChangeRoad?.Invoke();
        }

        characterValues.SetSpeedDistance(_currentSpeed, Distance);
    }

    Vector3 LeftRight(Vector3 newVelocity)
    {
        if (left)
        {
            if (!_roadLeftRight)
            {
                newVelocity.x = -StrafeSpeed;
            }
            else
            {
                newVelocity.y = -StrafeSpeed;
            }
        }
        if (right)
        {
            if (!_roadLeftRight)
            {
                newVelocity.x = StrafeSpeed;
            }
            else
            {
                newVelocity.y = StrafeSpeed;
            }
        }
        return newVelocity;
    }

    public void Jump()
    {
        rb.AddForce(transform.up * SpeedJump, ForceMode.Impulse);
    }

    public void BurnIntoWall()
    {
        onStopRun?.Invoke();
    }

    /// <summary>
    /// if run - variable is true, otherwise - false
    /// </summary>
    /// <param name="run"></param>
    public void RunOrIdle(bool run)
    {
        IsRun = run;

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
    }

    public void Restart()
    {
        onStartRun?.Invoke();
        _isRun = true;
        _isPause = false;
        _currentSpeed = StartSpeed;
        transform.position = new Vector3(0, 0, 0);
        currentBuff = BuffType.none;
        buffSpeed = 0;
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
        newVelocity.z = _currentSpeed;
        rb.velocity = newVelocity;
    }

    public void SetSpeedBuff(BuffType type)
    {
        var value = type switch
        {
            BuffType.SpeedBuff_1 => 2,
            BuffType.SpeedBuff_2 => 4,
            BuffType.SpeedBuff_3 => 6,
            BuffType.SpeedNerf_1 => -2,
            BuffType.SpeedNerf_2 => -4,
            BuffType.SpeedNerf_3 => -6,
            _ => 0
        };
        
        buffSpeed = value;
        currentBuff = type;

        timer.StartSpeed();
    }

    public void ResetBuffType()
    {
        currentBuff = BuffType.none;
        buffSpeed = 0;
    }

    public void SetRoadSwap(BuffType type)
    {
        transform.position = new Vector3(type == BuffType.RoadLeft ? -9.5f : 9.5f, transform.position.y, transform.position.z);
        _roadLeftRight = true;
    }
}
