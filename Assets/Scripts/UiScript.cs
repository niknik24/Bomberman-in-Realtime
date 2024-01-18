using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UiScript : MonoBehaviour
{
    private float timer = 180;
    public UnityEvent onTimeOut;

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            onTimeOut.Invoke();
        }

        GameObject obj = this.transform.GetChild(1).gameObject;
        TMPro.TextMeshProUGUI text = obj.GetComponent<TMPro.TextMeshProUGUI>();
        float min = Mathf.FloorToInt(timer / 60);
        float sec = Mathf.FloorToInt(timer % 60);
        string s;
        if (sec <10)
        {
            s = "0"+sec.ToString();
        }
        else
        {
            s =sec.ToString();
        }
        text.text = "Time: "+min.ToString()+":"+ s;
    }

    public void Restart()
    {
        timer = 180;
    }
}
