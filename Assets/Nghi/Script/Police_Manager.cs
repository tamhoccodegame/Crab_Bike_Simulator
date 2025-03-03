using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police_Manager : MonoBehaviour
{
    public static Police_Manager Instance;  // Singleton để dễ gọi từ NPC
    private List<Police_Behavior> policeList = new List<Police_Behavior>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void RegisterPolice(Police_Behavior police)
    {
        if (!policeList.Contains(police))
            policeList.Add(police);
    }

    public void AlertAllPolice(NPC_Behavior reporter)
    {
        foreach (var police in policeList)
        {
            police.ReceiveNPCReport(reporter);
        }
    }
}
