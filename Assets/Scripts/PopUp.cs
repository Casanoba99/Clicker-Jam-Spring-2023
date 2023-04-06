using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public Vector2 offset;
    [Space(5)]
    public string compradosTxt;
    public TextMeshProUGUI compradosTMP;
    public string produccionTxt;
    public string produccion1Txt;
    public TextMeshProUGUI produccionTMP;

    void Update()
    {
        Vector2 ratonPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = ratonPos + offset;
    }
}
