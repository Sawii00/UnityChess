using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHovering : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Text theText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        theText.fontStyle = FontStyle.Bold; //Or however you do your color
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theText.fontStyle = FontStyle.Normal; //Or however you do your color
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (theText.text == "Quit")
        {
            Application.Quit();
            return;
        }
        else if (theText.text == "Singleplayer")
        {
            SceneManager.LoadScene("TimeSelection", LoadSceneMode.Single);
        }
        else
            SceneManager.LoadScene(theText.text, LoadSceneMode.Single);
    }
}