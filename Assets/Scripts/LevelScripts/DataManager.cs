using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
    }


}
