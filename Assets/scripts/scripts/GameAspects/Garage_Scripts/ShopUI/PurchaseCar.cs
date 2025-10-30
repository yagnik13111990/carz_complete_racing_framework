using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public enum CarType { Drift , Race}
public class PurchaseCar : MonoBehaviour , IObserverOfSelectedCar
{
    [SerializeField] private TMP_Text Notice;
    [SerializeField] private TMP_Text Currency;
    [SerializeField] private TMP_Text AlreadyBought;

    [SerializeField] private GameObject DeniedMessage;
    [SerializeField] private GameObject WishMessage;

    [SerializeField] private Button BuyButton;
    [SerializeField] private Button CloseButtonForDeniedMessage;

    private CarScriptableObjData CurrentCarSOInfo;
    private CarInfo CarInfo;

  
    private float blinkSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        CurrencyTextUpdation();

        CloseButtonForDeniedMessage.onClick.AddListener(() => { DeniedMessage.SetActive(false); });

        BuyButton.onClick.AddListener(ClickEvent);

        ServiceLocator.Instance.GetService<InventoryManager>().OnPurchaseSucceed += ()=> { StartCoroutine(ShowWishMessage());  }; 


    }

    
    IEnumerator ShowWishMessage()
    {
        WishMessage.SetActive(true);
        CurrencyTextUpdation();
         yield return new WaitForSeconds(2f);

        WishMessage.SetActive(false);

     
    }
   
    void ClickEvent()
    {
        CarInfo = new CarInfoBuilder()
           .SetTorque(CurrentCarSOInfo.Torque)
           .SetMaximumSpeed(CurrentCarSOInfo.Maxspeed)
           .SetCarType(CurrentCarSOInfo.CarType)
           .SetCarID(CurrentCarSOInfo.CarID)
           .SetAcceleration(CurrentCarSOInfo.Acceleration)
           .SetCornering(CurrentCarSOInfo.Cornering)
           .SetCarName(CurrentCarSOInfo.CarName)
           .Build();

        if(!ServiceLocator.Instance.GetService<WalletManager>().AbleToBuy(CurrentCarSOInfo.Price))
        {
            DeniedMessage.SetActive(true);
            return;
        }

        ServiceLocator.Instance.GetService<WalletManager>().SpendMoney(CurrentCarSOInfo.Price);
        

        
        PurchaseEvent.RaisePurchaseEvent(CarInfo);
        
        
    }

    void CurrencyTextUpdation()
    {
        Currency.text = $"{ServiceLocator.Instance.GetService<WalletManager>().AvailableMoney} CR";
    }
    // Update is called once per frame

    void ButtonControl()
    {
        if(CurrentCarSOInfo != null)
        {
            if (ServiceLocator.Instance.GetService<InventoryManager>().AvailableCars.Exists(c => c.CarID == CurrentCarSOInfo.CarID))
            {
                BuyButton.gameObject.SetActive(false);
                AlreadyBought.gameObject.SetActive(true);

            }
            else
            {
                BuyButton.gameObject.SetActive(true);
                AlreadyBought.gameObject.SetActive(false);
            }
        }
    }
    public void UpdateStates(CarScriptableObjData data)
    {
        if (Notice.gameObject.activeInHierarchy)
        {
            Notice.gameObject.SetActive(false);
        }
        CurrentCarSOInfo = data;
       
    }

    public void NoticeControl()
    {
        float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);    
        Notice.color = new Color(22f / 255f, 226f / 255f, 0f / 255f, alpha);
    }
    private void Update()
    {
       ButtonControl();
        if (Notice.gameObject.activeInHierarchy)
        {
            NoticeControl();
        }
    }
}
