using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text time_text_white;
    public Text time_text_black;
    private float time_white = 900.0f;
    private float time_black = 900.0f;
    private bool whiteTurn = true;
    public PopupHandler popup;
    private bool stop = false;

    private float increment = 0.0f; 

    // Start is called before the first frame update
    private void Start()
    {
        time_white = PlayerPrefs.GetFloat("Time");
        time_black = time_white;
        increment = PlayerPrefs.GetFloat("Increment");

    }

    public bool WhiteTurn()
    {
        return whiteTurn;
    }

    public void SwitchTurn()
    {
        if (whiteTurn)
        {
            whiteTurn = false;
            time_white += increment;
        }
        else 
        {
            whiteTurn = true;
            time_black += increment;
        }
    }

    public void EndGameTimeout(Color color)
    {
        stop = true;
        popup.SetText(color.ToString() + " won by timeout!");
        popup.Show();
    }

    public void EndGameCheckmate(Color color)
    {
        stop = true;
        popup.SetText(color.ToString() + " won by Checkmate!");
        popup.Show();
    }
    public void EndGameDraw(Color color, string str)
    {
        stop = true;
        popup.SetText("Draw by "+str);
        popup.Show();
    }

    private string FormatTime(float seconds)
    {
        int mins = (int)seconds / 60;
        int secs = (int)seconds % 60;

        if (secs >= 10)
            return "" + mins + ":" + secs;
        else if (secs < 10)
            return "" + mins + ":0" + secs;
        else
            return "" + mins;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!stop)
        {
            if (time_black <= 0)
            {
                EndGameTimeout(Color.White);
            }
            else if (time_white <= 0)
            {
                EndGameTimeout(Color.Black);
            }
            else
            {

                time_text_black.text = FormatTime(time_black);
                time_text_white.text = FormatTime(time_white);
                if (whiteTurn)
                {
                    time_white -= Time.deltaTime;
                }
                else
                {
                    time_black -= Time.deltaTime;
                }
            }
        }
    }
}