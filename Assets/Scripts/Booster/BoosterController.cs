using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoosterController : MonoBehaviour
{

    TMP_Text UIText;
    public float count;

    private void Start()
    {
        UIText = transform.Find("Canvas").Find("UIText").GetComponent<TMP_Text>();
        UIText.text = "+" + count.ToString();
    }

}
