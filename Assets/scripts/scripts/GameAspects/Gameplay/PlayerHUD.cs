using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private RectTransform SpeedNeedle;
    [SerializeField] private TMP_Text SpeedMeter;
    [SerializeField] private TMP_Text UnitText;

    [SerializeField] private RawImage Minimap;
    [SerializeField] private GameObject SpeedoMeter;

    StringBuilder speedBuilder = new StringBuilder();
   
    private float lastspeed = -1;
    private float MinAngle = 0f;
    private float MaxAngle = 256f;

    private float ZAngle = 0;

    private void Start()
    {
        if (!(bool)ServiceLocator.Instance.GetService<SettingManager>().M_HUDs.HUDsSettings[HUDsSettingKey.SpeedoMeter])
        {
            SpeedoMeter.SetActive(false);

        }

        if (!(bool)ServiceLocator.Instance.GetService<SettingManager>().M_Gameplay.GameplaySettings[GameplaySettingKey.Map])
        {
            Minimap.gameObject.SetActive(false);
        }
    }
    public void UpdateSpeedometer(float speed , float maxspeed)
    {
        ZAngle = Mathf.Lerp(MinAngle, -MaxAngle, speed / maxspeed);

        SpeedNeedle.localRotation = Quaternion.Euler(0, 0 , ZAngle);  

        
        if(speed != lastspeed)
        {
            speedBuilder.Clear();

           
            SpeedMeter.text = speedBuilder.Append(Mathf.RoundToInt(speed)).ToString();
            lastspeed = speed;
        }
      
    }

    public void UpdateUnitText(string unit)
    {
        UnitText.text = unit;
    }


    

}
