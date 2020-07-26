using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupHandler : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Button btn;
    private string txt = "Text";
    void Start()
    {
        btn.onClick.AddListener(ReturnToMenu);
        GetComponentInChildren<Text>().text = txt;
        gameObject.SetActive(false);
    }

    public void SetText(string s)
    {
        txt = s;
    }

    public void Show()
    {
        GetComponentInChildren<Text>().text = txt;
        gameObject.SetActive(true);
    }
    private void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    
    
}
