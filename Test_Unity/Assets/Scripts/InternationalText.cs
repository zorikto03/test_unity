using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InternationalText : MonoBehaviour
{
    [SerializeField] string Ru;
    [SerializeField] string En;

    private void Start()
    {
        if (!string.IsNullOrEmpty(Language.Instance?.Lang))
        {
            GetComponent<TextMeshProUGUI>().text = Language.Instance.Lang switch
            {
                "en" => En,
                "ru" => Ru,
                _ => En
            };
        }        
    }

}
