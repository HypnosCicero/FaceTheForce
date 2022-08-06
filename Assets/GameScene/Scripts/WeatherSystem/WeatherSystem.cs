using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WeatherSystem : MonoBehaviour
{
    public GameObject backGround; 

    public WeatherType[] weatherGroups;
    public GameObject plane;
    private PhysicMaterial planeMaterial;//��ȡ�ذ����������

    public GameObject car;

    private float[] valueOfWeather = new float[5] { 30, 25, 20, 15, 12 };
    private string[] stringOfWeather = new string[5] { "Clear", "Mostly Clear", "Mostly Cloudy", "Rain", "Thunderstorm" };

    //�¶ȵĶ���
    private Temperature temperature;

    private void Start()
    {

        UniStormSystem.Instance.OnWeatherChangeEvent.AddListener(() => WhenChangeWeather());//�������ı�ʱ�����õĺ���
        planeMaterial = plane.GetComponent<MeshCollider>().material;

        
        temperature = new Temperature();
        temperature.OnVariableChange += WhenChangeTemperature;

        InvokeRepeating("RogueTemperature", 1, 60);
    }

    //�����¼�
    private void OnDestroy()
    {
        temperature.OnVariableChange -= WhenChangeTemperature;
    }

    /// <summary>
    /// �������仯ʱ�����������
    /// </summary>
    private void WhenChangeWeather()
    {
        print("�ı�������");
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
                    //print("�ı�ǰ�Ķ�Ħ����Ϊ" + planeMaterial.dynamicFriction+",��Ħ����Ϊ"+planeMaterial.staticFriction);
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
                    //print("���ڵ������ǣ�" + UniStormSystem.Instance.CurrentWeatherType.WeatherTypeName);
                    break;
                }
        }

    }
    /// <summary>
    /// �¶���
    /// </summary>
    private int ClockOfTemperature() {
        int result = 0;
        string CurrentWeatherTypeName = UniStormSystem.Instance.CurrentWeatherType.WeatherTypeName;//��ȡ��ǰ����������
        for (int i = 0; i < weatherGroups.Length; i++) {
            if (string.Equals(CurrentWeatherTypeName, stringOfWeather[i])) {//�ҵ��������ڵ�����
                result = i;
                if (valueOfWeather[i] != temperature.TemperValue)
                {
                    temperature.TemperValue = valueOfWeather[i];//����¶Ȳ����������ȸ�ֵ����
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
    /// һ����们
    /// </summary>
    private void WillSlip()
    {
        planeMaterial.dynamicFriction = 0.3f;
        planeMaterial.staticFriction = 0.3f;
        //print("�ı��   �Ķ�Ħ����Ϊ" + planeMaterial.dynamicFriction + "��Ħ����Ϊ" + planeMaterial.staticFriction);
    }


    /// <summary>
    /// ���������Ѹı�ֵ��״̬
    /// </summary>
    private void ResetPlaneAllState() {
        planeMaterial.dynamicFriction = 0.7f;
        planeMaterial.staticFriction = 0.7f;
        //print("��ԭ��------�Ķ�Ħ����Ϊ" + planeMaterial.dynamicFriction + "��Ħ����Ϊ" + planeMaterial.staticFriction);
    }
    /// <summary>
    /// ���¶ȸı�ʱ���е����������
    /// </summary>
    /// <param name="value">�ı���¶�</param>
    private void WhenChangeTemperature(float value)
    {
        string CurrentWeatherTypeName =UniStormSystem.Instance.CurrentWeatherType.WeatherTypeName;//��ȡ��ǰ����������
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
    /// ����Ӽ��¶�
    /// </summary>
    private void RogueTemperature() 
    {
        int rangeIndex = Random.Range(0,3);
        Debug.Log("��ʱ���¶�" + temperature.TemperValue + " ��ʱ��rangeIndex:" + rangeIndex);
        if (rangeIndex == 0)
        {
            Cooling(Random.Range(1, 5));
            Debug.Log("������䣬��ʱ���¶�" + temperature.TemperValue);
        }
        else if(rangeIndex == 1)
        {
            Warming(Random.Range(1, 5));
            Debug.Log("�����ů����ʱ���¶�" + temperature.TemperValue);
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
