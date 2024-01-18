using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void QuitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void PlayerDied()
    {
        this.gameObject.SetActive(true);
        GameObject state = this.gameObject.transform.GetChild(1).gameObject;
        TMPro.TextMeshProUGUI text = state.GetComponent<TMPro.TextMeshProUGUI>();
        text.text = "Ooops!";
    }

    public void Victory()
    {
        this.gameObject.SetActive(true);
        GameObject state = this.gameObject.transform.GetChild(1).gameObject;
        TMPro.TextMeshProUGUI text = state.GetComponent<TMPro.TextMeshProUGUI>();
        text.text = "Congratulations!";
    }
}
