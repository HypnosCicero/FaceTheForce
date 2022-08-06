using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WeatherSystem : MonoBehaviour
{
    public GameObject backGround; 

    public WeatherType[] weatherGroups;
    public GameObject plane;
    private PhysicMaterial planeMaterial;//获取地板的物理属性

    public GameObject car;

    private float[] valueOfWeather = new float[5] { 30, 25, 20, 15, 12 };
    private string[] stringOfWeather = new string[5] { "Clear", "Mostly Clear", "Mostly Cloudy", "Rain", "Thunderstorm" };

    //温度的对象
    private Temperature temperature;

    private void Start()
    {

        UniStormSystem.Instance.OnWeatherChangeEvent.AddListener(() => WhenChangeWeather());//当天气改变时所调用的函数
        planeMaterial = plane.GetComponent<MeshCollider>().material;

        
        temperature = new Temperature();
        temperature.OnVariableChange += WhenChangeTemperature;

        InvokeRepeating("RogueTemperature", 1, 60);
    }

    //测试事件
    private void OnDestroy()
    {
        temperature.OnVariableChange -= WhenChangeTemperature;
    }

    /// <summary>
    /// 当天气变化时调用这个方法
    /// </summary>
    private void WhenChangeWeather()
    {
        print("改变天气了");
        int index=ClockOfTemperature();
        car.GetComponent<SuperSportsCar>().isHeet = false;
        switch (index)
        {
            case 0: 
                {
                    MayExplode();
                    break;
                }
            case 3:
                {
                    //print("改变前的动摩擦力为" + planeMaterial.dynamicFriction+",静摩擦力为"+planeMaterial.staticFriction);
                    WillSlip();
                    break;
                }
            case 4:
                {
                    WillSlip();
                    break;
                }
            default:
                {
                    ResetPlaneAllState();
                    //print("现在的天气是：" + UniStormSystem.Instance.CurrentWeatherType.WeatherTypeName);
                    break;
                }
        }

    }
    /// <summary>
    /// 温度锁
    /// </summary>
    private int ClockOfTemperature() {
        int result = 0;
        string CurrentWeatherTypeName = UniStormSystem.Instance.CurrentWeatherType.WeatherTypeName;//获取当前天气的名称
        for (int i = 0; i < weatherGroups.Length; i++) {
            if (string.Equals(CurrentWeatherTypeName, stringOfWeather[i])) {//找到现在所在的天气
                result = i;
                if (valueOfWeather[i] != temperature.TemperValue)
                {
                    temperature.TemperValue = valueOfWeather[i];//如果温度不等则进行相等赋值操作
                }
                break;
            }
        }
        return result;
    }

    private void MayExplode()
    {
        car.GetComponent<SuperSportsCar>().isHeet = true;
    }

    /// <summary>
    /// 一定会变滑
    /// </summary>
    private void WillSlip()
    {
        planeMaterial.dynamicFriction = 0.3f;
        planeMaterial.staticFriction = 0.3f;
        //print("改变后   的动摩擦力为" + planeMaterial.dynamicFriction + "静摩擦力为" + planeMaterial.staticFriction);
    }


    /// <summary>
    /// 重置所有已改变值的状态
    /// </summary>
    private void ResetPlaneAllState() {
        planeMaterial.dynamicFriction = 0.7f;
        planeMaterial.staticFriction = 0.7f;
        //print("还原后------的动摩擦力为" + planeMaterial.dynamicFriction + "静摩擦力为" + planeMaterial.staticFriction);
    }
    /// <summary>
    /// 当温度改变时进行调用这个方法
    /// </summary>
    /// <param name="value">改变的温度</param>
    private void WhenChangeTemperature(float value)
    {
        string CurrentWeatherTypeName =UniStormSystem.Instance.CurrentWeatherType.WeatherTypeName;//获取当前天气的名称
        for (int i = 0; i < weatherGroups.Length; i++) {
            if (temperature.TemperValue == valueOfWeather[i] &&
                string.Equals(CurrentWeatherTypeName,stringOfWeather[i])==false)
            {
                UniStormManager.Instance.ChangeWeatherWithTransition(weatherGroups[i]);
                break;
            } 
        }
    }

    /// <summary>
    /// 随机加减温度
    /// </summary>
    private void RogueTemperature() 
    {
        int rangeIndex = Random.Range(0,3);
        Debug.Log("此时的温度" + temperature.TemperValue + " 此时的rangeIndex:" + rangeIndex);
        if (rangeIndex == 0)
        {
            Cooling(Random.Range(1, 5));
            Debug.Log("进入变冷，此时的温度" + temperature.TemperValue);
        }
        else if(rangeIndex == 1)
        {
            Warming(Random.Range(1, 5));
            Debug.Log("进入变暖，此时的温度" + temperature.TemperValue);
        }
    }
    private void Warming(float w)
    {
        temperature.TemperValue = temperature.TemperValue+w;
    }
    public void Cooling(float c)
    {
        temperature.TemperValue = temperature.TemperValue - c;
    }

}
