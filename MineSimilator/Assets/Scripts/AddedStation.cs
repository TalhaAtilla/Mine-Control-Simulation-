using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddedStation : MonoBehaviour
{

    private RaycastHit hit;
    private Vector3 movePoint;


    [SerializeField]
    private LayerMask layerMask;

    private bool isBreak = false;

    private int currentSaveStation;

    public List<GameObject> gameObjects = new List<GameObject>();

    public bool IsBreak
    {
        get
        {
            return isBreak;
        }
        set
        {
            isBreak = value;

        }
    }

    private void Update()
    {
        if (isBreak == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000.0f, layerMask))
            {
                transform.position = hit.point;

            }
            if (Input.GetMouseButtonDown(0))
            {
                isBreak = true;
                PlayerPrefs.SetFloat("posx" + PlayerPrefs.GetInt("currentSaveStation"), transform.position.x);
                PlayerPrefs.SetFloat("posy" + PlayerPrefs.GetInt("currentSaveStation"), transform.position.y);
                PlayerPrefs.SetFloat("posz" + PlayerPrefs.GetInt("currentSaveStation"), transform.position.z);
                //kayýt burda býraktýðýmýz an çalýþacak
            }
        }

        Debug.Log(PlayerPrefs.GetInt("currentSaveStation"));
    }

}
