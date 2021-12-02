using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using root;

public class SingletonReferences : MonoBehaviour
{
    public MasterManager _masterManager;

    void Start()
    {
        //MasterManager.AdmobManager.CustomAwake();
        //MasterManager.GetPlayerDataManager.CustomAwake();
    }

    private void Awake() {
        Debug.Log("Loading master manager..");
        MasterManager mm = MasterManager.Instance;
    }
}
