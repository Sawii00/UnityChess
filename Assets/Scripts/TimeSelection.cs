using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeSelection : MonoBehaviour
{
    // Start is called before the first frame update
    private Dropdown dd;

    private float[] times = { 1 * 60, 3 * 60, 15 * 60, 30 * 60 };

    private void Start()
    {
        dd = FindObjectOfType<Dropdown>();
        GetComponent<Button>().onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        float selected = times[dd.value];
        PlayerPrefs.SetFloat("Time", selected);
        SceneManager.LoadScene("Singleplayer", LoadSceneMode.Single);
    }
}