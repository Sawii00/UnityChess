using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class TimeSelection : MonoBehaviour
{
    // Start is called before the first frame update
    public Dropdown dd_time;
    public Dropdown dd_increment;

    private float[] times = { 1 * 60, 3 * 60, 15 * 60, 30 * 60 };
    private float[] increments = {0, 2, 3, 5, 10, 20 };

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        float selected = times[dd_time.value];
        float increment = increments[dd_increment.value];
        PlayerPrefs.SetFloat("Time", selected);
        PlayerPrefs.SetFloat("Increment", increment);
        SceneManager.LoadScene("Singleplayer", LoadSceneMode.Single);
    }
}