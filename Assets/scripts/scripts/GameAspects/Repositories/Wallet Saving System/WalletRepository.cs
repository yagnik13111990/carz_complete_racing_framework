using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WalletData
{
    public int AvailableMoney;
}
public class WalletRepository : IWalletRepository                                                                 
{
    string FilePath = "GameJsonDataFiles/Wallet.json";
    public void SaveWalletData(int Money)
    {
        string path = Path.Combine(Application.persistentDataPath, FilePath);
        WalletData data = new WalletData() { AvailableMoney = Money};

        string content = JsonConvert.SerializeObject(data);

        File.WriteAllText(path, content);
    }

    public WalletData LoadWalletData()
    {
        string path = Path.Combine(Application.persistentDataPath, FilePath);
        if (!File.Exists(path)) return new WalletData() { AvailableMoney = 20000 };

        string content = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<WalletData>(content) ?? new WalletData { AvailableMoney = 20000 };
       
    }
}
