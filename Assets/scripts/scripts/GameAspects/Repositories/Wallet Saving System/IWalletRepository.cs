using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IWalletRepository
{
    void SaveWalletData(int Money);

    WalletData LoadWalletData();
}
