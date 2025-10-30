using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class WalletManager 
{
    private int _AvailableMoney ;
    public int AvailableMoney => _AvailableMoney;
    private WalletData _walletData ;

    private  WalletRepository _walletRepository ;
    public WalletManager()
    {
        _walletRepository = new WalletRepository();
        _walletData = _walletRepository.LoadWalletData();
        _AvailableMoney = _walletData.AvailableMoney ;
    }

    public  bool AbleToBuy(int CarPrice)
    {
        return CarPrice <= _AvailableMoney;
    }

    public  void SpendMoney(int Money)
    {
        _AvailableMoney -= Money;
        _walletRepository.SaveWalletData(_AvailableMoney);
    }
    public  void Addcoin (int Money)
    {
        _AvailableMoney += Money;
        _walletRepository.SaveWalletData(_AvailableMoney);
    }


    
}
