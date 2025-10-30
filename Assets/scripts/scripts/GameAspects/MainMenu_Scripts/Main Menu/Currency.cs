using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Currency : MonoBehaviour
{
    private TMP_Text currencyText;

    private void Start()
    {
        currencyText = GetComponentInChildren<TMP_Text>();
        currencyText.text = $"{ServiceLocator.Instance.GetService<WalletManager>().AvailableMoney} CR";
    }
}
