using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Language : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern string GetLanguage();


    string _lang = string.Empty;
    public string Lang => _lang;

    static Language _instance;
    public static Language Instance => _instance;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        try
        {
            _lang = GetLanguage();
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
