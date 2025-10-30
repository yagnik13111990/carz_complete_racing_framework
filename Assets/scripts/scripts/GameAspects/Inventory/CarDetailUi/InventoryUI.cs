using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI :MonoBehaviour
{
    [SerializeField]private TMP_Text MaxSpeedText;

    [SerializeField]private TMP_Text TorqueText;

    [SerializeField]private TMP_Text NameText;

    [SerializeField]private TMP_Text AccelerationText;

    [SerializeField]private TMP_Text CorneringText;


    [SerializeField] private Slider MaxSpeedSlider;

    [SerializeField] private Slider TorqueSlider;

    [SerializeField] private Slider AccelerationSlider;

    [SerializeField] private Slider CorneringSlider;


    public void UpdateMaxSpeedText(int max)
    {
        MaxSpeedSlider.value = max;
        MaxSpeedText.text = max.ToString();
    }

    public void UpdateTorqueText(int tor)
    {
        TorqueSlider.value = tor;
        TorqueText.text = tor.ToString();
    }

    public void UpdateNameText(string name)
    {
        NameText.text = name;
    }

    public void UpdateAccelerationText(float acc)
    {
        AccelerationSlider.value = acc;
        AccelerationText.text = acc.ToString();
    }
    public void UpdateCorneringText(float cor)
    {
        CorneringSlider.value = cor;
        CorneringText.text = cor.ToString();
    }

}
