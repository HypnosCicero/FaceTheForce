using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 车
/// </summary>
public class SuperSportsCar : MonoBehaviour
{
    //车辆所需要的属性：
    public string carName= "Ferrari 296 gtb";//车名
    public float quality=1470;//质量单位：kg
    public float friction;//摩擦力
    public int numForGear=8;//档位的数量。
    private  float[] forwordForce;
    private float[] forwordSpeed;
    public GameObject lightSystem;

    public GameObject S2Controller;
    private Scene2Contoller S2C;

    //降速所需要的属性
    public AnimationCurve curve;
    private float x;
    //public float duration = 1;//持续时间


    private float horizontal;
    private float vertical;
    
    private WheelCollider[] wheelCollider;
    public float maxDigree = 200;
    //private float maxMotor = 300;
    private int gear = 0;
    private int gearTempF = 1;
    private int gearTempB = -1;

    private Rigidbody carRigidBody;

    public Transform startPosition;

    public bool isHit = false;
    public bool isHeet = false;
    public bool isWin = false;

    private void Start()
    {
        CreatMaxForwardForce();
        CreatMaxForwardSpeed();
        wheelCollider = transform.GetChild(2).GetComponentsInChildren<WheelCollider>();
        carRigidBody = this.GetComponent<Rigidbody>();
        this.transform.position = startPosition.position;//开头重置位置
        this.transform.rotation = startPosition.rotation;
        isHit = false;
        isHeet = false;
        isWin = false;
        lightSystem.SetActive(false);
        S2C = S2Controller.GetComponent<Scene2Contoller>();
    }
    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");//-1 ~ 1之间
        vertical = Input.GetAxis("Vertical");
        Move();
        //print("此时的速度：" + carRigidBody.velocity);
    }
    private void Update()
    {
        ChangeGear();

        if (isHit)
        {
            IsHited();
        }
        if (isHeet) 
        {
            JudgmentPuncture();
        }
        if (isWin)
        {
            StartCoroutine(IsWining());
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (S2C.backGround.activeSelf == false)
            {
                UniStormManager.Instance.SetMusicVolume(0);
                Time.timeScale = 0;
                S2C.BackTheMenu();
            }
            else {
                UniStormManager.Instance.SetMusicVolume(1);
                Time.timeScale = 1;
                S2C.backGround.SetActive(false);
                S2C.textString.SetActive(true);
            }
        }
    }
    
    /// <summary>
    /// 移动以及旋转
    /// </summary>
    private void Move()
    {
        
        for (int i = 0; i < 4; i++)//汽车动力
        {
            if (i < 2)
            {
                wheelCollider[i].steerAngle = horizontal * maxDigree;//转向
            }
            wheelCollider[i].motorTorque = forwordForce[gear] * vertical;//最大速度。
        }
    }

    
    /// <summary>
    /// 用于创建每个档位最大值牵引力的方法
    /// </summary>
    private void CreatMaxForwardForce() {
        forwordForce = new float[numForGear];
        //这里是驱动力而不是速度，所以降档的时候需要一定的反制动力让速度降下来，
        //并且不能以速度作为衡量是否降速的标准。需要找一个新的参数进行调节
        forwordForce[0] = 100f;//15km/h //4166.7fm/s 
        forwordForce[1] = 200f;//25km/h //6944.5fm/s 
        forwordForce[2] = 315f;//45km/h //12500fm/s 
        forwordForce[3] = 400f;//75km/h //20833.4m/s 
        forwordForce[4] = 450f;//130km/h //36111.2m/s 
        forwordForce[5] = 550f;//190km/h //52777.8m/s
        forwordForce[6] = 650f;//250km/h //69444.4m/s
        forwordForce[7] = 740f;//330km/h //91666.7m/s

    }
    /// <summary>
    /// 用于创建每个档位最大值速度的方法
    /// </summary>
    private void CreatMaxForwardSpeed()
    {
        forwordSpeed = new float[numForGear];
        //这里是驱动力而不是速度，所以降档的时候需要一定的反制动力让速度降下来，
        //并且不能以速度作为衡量是否降速的标准。需要找一个新的参数进行调节
        forwordSpeed[0] = 15f;//15km/h //4166.7fm/s 
        forwordSpeed[1] = 25f;//25km/h //6944.5fm/s 
        forwordSpeed[2] = 45;//45km/h //12500fm/s 
        forwordSpeed[3] = 75f;//75km/h //20833.4m/s 
        forwordSpeed[4] = 130f;//130km/h //36111.2m/s 
        forwordSpeed[5] = 190f;//190km/h //52777.8m/s
        forwordSpeed[6] = 250f;//250km/h //69444.4m/s
        forwordSpeed[7] = 330f;//330km/h //91666.7m/s

    }
    //倒挡 0~60km/h
    /// <summary>
    /// 用于换挡的方法
    /// </summary>
    private void ChangeGear() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {//按下一次
            gear+=gearTempF;
            if (gear == 7)
            {
                gearTempF = 0;
            }
            gearTempB = -1;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            gear += gearTempB;
            if (gear == 0)
            {
                gearTempB = 0;
            }
            gearTempF = 1;
            if (carRigidBody.velocity.z > forwordSpeed[gear])
            {
                ChangeGearSpeedDown();
            }
        }
        if (Input.GetKeyUp(KeyCode.E)) {
            LightAndOFF();
        }
        
    }

    /// <summary>
    /// 换挡速度降低方法
    /// </summary>
    private void ChangeGearSpeedDown()
    {
        //print("进入这个方法");
        for (int i = 0; i < 4; i++)//汽车动力
        {
            wheelCollider[i].brakeTorque = Mathf.Lerp(60,20, curve.Evaluate(x));
            //print("当前速度为=" + wheelCollider[i].motorTorque);
        }
        //print("当前档位为：" + gear + " 当前档位速度为：" + forwordForce[gear]);
    }

    /// <summary>
    /// 灯光开闭系统
    /// </summary>
    private void LightAndOFF() 
    {
        if (lightSystem.activeInHierarchy == false)
        {
            lightSystem.SetActive(true);
        }
        else {
            lightSystem.SetActive(false);
        }
    }

    /// <summary>
    /// 被击中
    /// </summary>
    private void IsHited() {
        UniStormManager.Instance.SetMusicVolume(0);
        Time.timeScale = 0;//暂停游戏
        S2C.BadGameOver();
        if (Input.anyKey)
        {
            S2C.BackTheMenu();
            isHit = false;
        }
    }

    /// <summary>
    /// 判断是否爆胎方法
    /// </summary>
    private void JudgmentPuncture() {
        if (carRigidBody.velocity.z >= 100f) {
            UniStormManager.Instance.SetMusicVolume(0);
            Debug.Log("进入Puncture");
            Time.timeScale = 0;//暂停游戏
            S2C.BadGameOver();
            if (Input.anyKey)
            {
                S2C.BackTheMenu();
                isHit = false;
            }
        }
    }

    private IEnumerator IsWining() {
        UniStormManager.Instance.SetMusicVolume(0);
        
        Time.timeScale = 0;//暂停游戏
        S2C.WinGameOver();
        //yield return new WaitForSeconds(1);
        yield return 1;
        if (Input.GetMouseButton(0))
        {
            S2C.BackTheMenu();
            isWin = false;
        }
    }
}
