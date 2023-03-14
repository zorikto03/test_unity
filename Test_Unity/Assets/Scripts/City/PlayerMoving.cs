using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] float MaxSpeed = 20;
    [SerializeField] float StrafeSpeed = 10;
    [SerializeField] float SpeedJump = 10;
    [SerializeField] Camera cam;
    [SerializeField] Motor motor;

    public static Action onChangeRoad;
    public delegate void BrakeEvent(bool isBrake);
    public static event BrakeEvent OnBrakeEvent;

    BuffTimer timer;
    Rigidbody rb;
    bool left = false;
    bool right = false;
    bool accelerate = false;
    bool brake = false;
    bool buff = false;
    bool _isPause;
    bool _isRun = true;
    bool _roadLeftRight = false;//indicator of road swaping to left or right
    CharacterValues characterValues;
    PlayerDistanceManager playerDistanceManager;
    float _currentSpeed = 0f;
    int buffSpeed;
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

    public float Distance => transform.position.z / 10;
    public float CurrentSpeed => _currentSpeed;
    public float GetMaxSpeed => motor.MaxSpeed;
    public bool Deceleration => motor.Deceleration; //проверка снижается скорость или нет
    public bool Accelerate => accelerate;
    public bool Brake => brake;
    public bool Buff => buff;
    public int BuffSpeed => buffSpeed;
    public float MotorTorgue => motor.CurrentMotorTorgue;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterValues = FindObjectOfType<CharacterValues>();
        playerDistanceManager = GetComponent<PlayerDistanceManager>();
        timer = GetComponent<BuffTimer>();

        motor = new();
    }

    private void OnEnable()
    {
        BuffTimer.OnSpeedTimerStoped += ResetBuffType;
    }

    private void OnDisable()
    {
        BuffTimer.OnSpeedTimerStoped -= ResetBuffType;
    }

    void Update()
    {
        left = Input.GetKey(KeyCode.A);
        
        right = Input.GetKey(KeyCode.D);
        
        accelerate = Input.GetKey(KeyCode.W);

        if (Input.GetKeyDown(KeyCode.S))
        {
            brake = true;
            motor.MotorTorgue(accelerate, brake, buffSpeed, buff);
            OnBrakeEvent?.Invoke(brake);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            brake = false;
            motor.MotorTorgue(accelerate, brake, buffSpeed, buff);
            OnBrakeEvent?.Invoke(brake);
        }

        if (currentBuff != BuffType.none && !buff)
        {
            buff = Input.GetKey(KeyCode.Space);
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

    #region PlayerMoving
    
    void MovingByTransform()
    {
        Vector3 newVelocity = LeftRight(rb.velocity);

        newVelocity.z = Speed();
        rb.velocity = newVelocity;

        LimitPosRot();

        playerDistanceManager.SetDistanceToPoints(Distance);
        characterValues.SetSpeed(_currentSpeed);
    }
    float Speed()
    {
        motor.MotorTorgue(accelerate, brake, buffSpeed, buff);
        var speed = motor.CalculateSpeed();
        return _currentSpeed = speed != 0 ? speed : _currentSpeed;
    }

    void LimitPosRot()
    {
        var newPos = transform.position;
        if (!_roadLeftRight)
        {
            newPos.x = Mathf.Clamp(newPos.x, -4f, 4f);
        }
        else
        {
            newPos.y = Mathf.Clamp(newPos.y, -4f, 4f);
        }
        transform.position = newPos;
    }

    float CoefBySpeed()
    {
        float coef = GetMaxSpeed / CurrentSpeed / 2 * StrafeSpeed;
        coef = Mathf.Clamp(coef, 1, 40);
        return Time.deltaTime * coef;
    }

    Vector3 LeftRight(Vector3 newVelocity)
    {
        if (left)
        {
            if (!_roadLeftRight)
            {
                newVelocity.x -= CoefBySpeed();
                if (newVelocity.x < -StrafeSpeed)
                {
                    newVelocity.x = -StrafeSpeed;
                }
            }
            else
            {
                newVelocity.y -= CoefBySpeed();
                if (newVelocity.y < -StrafeSpeed)
                {
                    newVelocity.y = -StrafeSpeed;
                }
            }
        }
        if (right)
        {
            if (!_roadLeftRight)
            {
                newVelocity.x += CoefBySpeed();
                if (newVelocity.x > StrafeSpeed)
                {
                    newVelocity.x = StrafeSpeed;
                }
            }
            else
            {
                newVelocity.y += CoefBySpeed();
                if (newVelocity.x > StrafeSpeed)
                {
                    newVelocity.x = StrafeSpeed;
                }
            }
        }
        return newVelocity;
    }

    public void BarrierEncounter()
    {
        motor.TorgueAfterHitCar();
    }

    public void Jump()
    {
        if (buff && currentBuff == BuffType.Jump)
        {
            rb.AddForce(transform.up * SpeedJump * 1000, ForceMode.Impulse);
            buff = false;
            //currentBuff = BuffType.none;
        }
    }

    #endregion


    /// <summary>
    /// if run - variable is true, otherwise - false
    /// </summary>
    /// <param name="run"></param>
    public void RunOrIdle(bool run)
    {
        IsRun = run;
    }

    public void ContinuePlaying()
    {
        currentBuff = BuffType.none;
        buff = false;
        buffSpeed = 0;
        rb.isKinematic = false;
        motor.Restart();
        characterValues.EnableRealStats();
        timer.StopSoundTimer();
    }

    public void Restart()
    {
        ContinuePlaying();
        _isPause = false;
        transform.position = new Vector3(0, 0, 0);
        
        playerDistanceManager.Restart();
    }

    public void SetSpeedBuff(BuffType type)
    {
        if (!buff && currentBuff != type)
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
    }

    public void SetJumpBuff()
    {
        currentBuff = BuffType.Jump;
    }

    public void ResetBuffType()
    {
        currentBuff = BuffType.none;
        buffSpeed = 0;
        buff = false;
    }

    public void SetRoadSwap(BuffType type)
    {
        transform.position = new Vector3(type == BuffType.RoadLeft ? -9.5f : 9.5f, transform.position.y, transform.position.z);
        _roadLeftRight = true;
    }

    public void GameOver()
    {
        motor.Restart();
        timer.StartSoundTimer();
        playerDistanceManager.GameOver();
        rb.isKinematic = true;
    }

}
