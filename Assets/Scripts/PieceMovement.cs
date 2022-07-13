using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;
using System.Text.RegularExpressions;

public class PieceMovement : MonoBehaviour {
    public Camera cam;
    private BoardManager bm;
    private UnityEngine.Color mouseOverColor;
    private UnityEngine.Color originalColor;
    private bool dragging = false;
    private int mask = 1 << 8;

    private int board_x, board_y;
    private int dest_x, dest_y;

    


    void Start()
    {
        mask = ~mask;
        originalColor = GetComponent<Renderer>().material.color;
        mouseOverColor = UnityEngine.Color.yellow;
        bm = FindObjectOfType<BoardManager>();

        Camera[] CamArr = FindObjectsOfType<Camera>();

        if (Regex.IsMatch(name, ".*Light.*"))
        {
            if (CamArr[0].gameObject.name == "Main Camera White")
                cam = CamArr[0];
            else
                cam = CamArr[1];
        }
        else
        {
            if (CamArr[0].gameObject.name == "Main Camera Black")
                cam = CamArr[0];
            else
                cam = CamArr[1];
        }
    }


    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = mouseOverColor;
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = originalColor;
    }

    void OnMouseDown()
    {
        dragging = true;
        board_x = (int)transform.position.x;
        board_y = (int)transform.position.z;
    }

    void OnMouseUp()
    {
        bool eaten;
        dragging = false;
        dest_x = (int)transform.position.x;
        dest_y = (int)transform.position.z;


        if (!bm.Move(board_x, board_y, dest_x, dest_y, out eaten))
        {
            
            transform.position = new UnityEngine.Vector3(board_x, 0, board_y);

        }

        if (eaten)
        {
            this.gameObject.SetActive(false);
            RaycastHit hit;
            if (Physics.Raycast(new UnityEngine.Vector3(dest_x, 10, dest_y), -UnityEngine.Vector3.up, out hit, Mathf.Infinity, 1 << 8))
            {
                hit.collider.gameObject.SetActive(false);
            }
            this.gameObject.SetActive(true);

        }

    }

    void Update()
    {
        if (dragging)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                transform.position = new UnityEngine.Vector3(hit.collider.transform.position.x, 0, hit.collider.transform.position.z);
            }
            else 
            {
                //Debug.Log("Nothing");
            }
            
        }
    }
}
