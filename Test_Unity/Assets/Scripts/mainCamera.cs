using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class mainCamera : MonoBehaviour
{
    [SerializeField] float OffsetY = 3;
    [SerializeField] float OffsetZ = -4;
    [SerializeField] int RotationX = 27;
    [SerializeField] float SpeedRotate = 10;
    [SerializeField] float SpeedMoveToPlace = 10;

    //PostProcessVolume PostProcessing;
    //Vignette _vignette;
    //ChromaticAberration _chromaticAberration;
    //DepthOfField _dof;
    
    PlayerMoving player;
    Vector3 basicRot;
    bool left, right, isShake;
    float _eulerX, _eulerZ;
    int minFOV = 60;
    int maxFOV = 70;
    int buffSpeedFOV = 90;
    Camera cam;
    Vector3 _distanceFromObject;

    private void Start()
    {
        cam = GetComponent<Camera>();
        //PostProcessing = GetComponent<PostProcessVolume>();
        //PostProcessing.TryGetComponent(out _vignette);
        //PostProcessing.TryGetComponent(out _chromaticAberration);
        //PostProcessing.TryGetComponent(out _dof);

        player = FindObjectOfType<PlayerMoving>();
        basicRot = transform.eulerAngles;
        _eulerX = RotationX;
        _distanceFromObject = new Vector3(0, OffsetY, OffsetZ);
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

        _eulerX = RotationX;
        _distanceFromObject = new Vector3(0, OffsetY, OffsetZ);
    }

    private void LateUpdate()
    {
        Vector3 positionToGo = player.transform.position + _distanceFromObject;
        Vector3 smoothPosition = Vector3.Lerp(a: transform.position, b: positionToGo, t: 1F);
        transform.position = smoothPosition;
        

        if (!cam.orthographic)
        {
            RotateCam();
            CHangeFOV();
        }
        else
        {
            transform.eulerAngles = new Vector3(10, 0, 0);
        }
    }


    float CalculateEulerXByCarSpeed()
    {
        float coef = player.GetMaxSpeed / player.CurrentSpeed / 10 * SpeedRotate;
        coef = Mathf.Clamp(coef, 1, 10);
        return Time.deltaTime * coef;
    }

    void RotateCam()
    {
        //вращение камеры по оси z при перемещениях вправо-влево,
        //скорость вращения зависит от скорости авто
        if (left)
        {
            _eulerZ -= CalculateEulerXByCarSpeed();
        }
        else if (right)
        {
            _eulerZ += CalculateEulerXByCarSpeed();
        }
        else
        {
            if (Mathf.Round(_eulerZ) > 0)
            {
                _eulerZ -= CalculateEulerXByCarSpeed();
            }
            else if (Mathf.Round(_eulerZ) < 0)
            {
                _eulerZ += CalculateEulerXByCarSpeed();
            }
        }

        if (Mathf.Round(transform.eulerAngles.x) != _eulerX)
        {
            if(_eulerX < transform.eulerAngles.x)
            {
                transform.Rotate(new Vector3(-15 * Time.deltaTime, 0, 0));
            }
            else if (transform.eulerAngles.x < _eulerX)
            {
                transform.Rotate(new Vector3(15 * Time.deltaTime, 0, 0));
            }
        }

        _eulerZ = Mathf.Clamp(_eulerZ, -30f, 30f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, _eulerZ);
    }

    public void CameraDown()
    {
        OffsetY = 1;
        _eulerX = 0;
    }

    public void CameraUp()
    {
        OffsetY = 3;
        _eulerX = 27;
    }

    public void Restart()
    {
        _eulerX = RotationX;
        transform.eulerAngles = new Vector3(_eulerX, 0, 0);
        transform.position = new Vector3(transform.position.x, OffsetY, transform.position.z);
    }

    void CHangeFOV()
    {

        if (player.Accelerate && !player.Brake)
        {
            if (player.Buff)
            {
                if (cam.fieldOfView < buffSpeedFOV)
                {
                    cam.fieldOfView += buffSpeedFOV / cam.fieldOfView / (player.BuffSpeed * 5);
                }
            }
            else
            {
                if (cam.fieldOfView < maxFOV && player.CurrentSpeed > player.GetMaxSpeed / 2)
                {
                    if (cam.fieldOfView + maxFOV / cam.fieldOfView / 7 >= maxFOV)
                    {
                        cam.fieldOfView = maxFOV;
                    }
                    else
                    {
                        cam.fieldOfView += maxFOV / cam.fieldOfView / 7;
                    }
                }
                else if (cam.fieldOfView > maxFOV)
                {
                    cam.fieldOfView -= maxFOV / cam.fieldOfView / 5;
                }
            }
        }
        else if (!player.Accelerate)
        {
            if (cam.fieldOfView > minFOV && player.CurrentSpeed < player.GetMaxSpeed / 3 * 2)
            {
                if (!player.Brake)
                {
                    cam.fieldOfView -= maxFOV / cam.fieldOfView / 10;
                }
                else
                {
                    cam.fieldOfView -= maxFOV / cam.fieldOfView / 5;
                }
            }
        }
    }

    public void ShakeCamera()
    {
        StartCoroutine(Shake(0.7f, 0.5f, 10f));
    }
    private IEnumerator Shake(float duration, float magnitude, float noize)
    {
        isShake = true;
        //Инициализируем счётчиков прошедшего времени
        float elapsed = 0f;
        //Сохраняем стартовую локальную позицию
        Vector3 startPosition = transform.localPosition;
        //Генерируем две точки на "текстуре" шума Перлина
        Vector2 noizeStartPoint0 = Random.insideUnitCircle * noize;
        Vector2 noizeStartPoint1 = Random.insideUnitCircle * noize;

        //Выполняем код до тех пор пока не иссякнет время
        while (elapsed < duration)
        {
            //Генерируем две очередные координаты на текстуре Перлина в зависимости от прошедшего времени
            Vector2 currentNoizePoint0 = Vector2.Lerp(noizeStartPoint0, Vector2.zero, elapsed / duration);
            Vector2 currentNoizePoint1 = Vector2.Lerp(noizeStartPoint1, Vector2.zero, elapsed / duration);
            //Создаём новую дельту для камеры и умножаем её на длину дабы учесть желаемый разброс
            Vector2 cameraPostionDelta = new Vector2(Mathf.PerlinNoise(currentNoizePoint0.x, currentNoizePoint0.y), Mathf.PerlinNoise(currentNoizePoint1.x, currentNoizePoint1.y));
            cameraPostionDelta *= magnitude;

            //Перемещаем камеру в нувую координату
            startPosition.z = transform.localPosition.z;
            //startPosition.y = transform.localPosition.y;
            //startPosition.x = transform.localPosition.x;
            transform.localPosition = startPosition + (Vector3)cameraPostionDelta;

            //Увеличиваем счётчик прошедшего времени
            elapsed += Time.deltaTime;
            //Приостанавливаем выполнение корутины, в следующем кадре она продолжит выполнение с данной точки
            yield return null;
        }
        isShake = false;
    }
}
