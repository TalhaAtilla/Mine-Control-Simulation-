using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddedReciever : MonoBehaviour
{
   
    private RaycastHit hit;
    private Vector3 movePoint;


    [SerializeField]
    private LayerMask layerMask;

    private bool isBreak=false;

    public int Sayı;

    private int currentSaveReciever;

    [SerializeField]
    public RecieverManager RecieverManagg;

    public List<GameObject> gameObjects= new List<GameObject>();

    public bool IsBreak { get
    {
        return isBreak;
    }
    set
    {
        isBreak=value;

    } }
    
    private void Update() 
    {
        if(isBreak==false)
        {
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if(Physics.Raycast(ray,out hit,1000.0f,layerMask))
            {
                transform.position=hit.point;
                
            }
            if(Input.GetMouseButtonDown(0))
            {
                isBreak =true;
                
                PlayerPrefs.SetFloat("Rposx" +PlayerPrefs.GetInt("currentSaveReciever"),transform.position.x);
                PlayerPrefs.SetFloat("Rposy"+PlayerPrefs.GetInt("currentSaveReciever"),transform.position.y);
                PlayerPrefs.SetFloat("Rposz"+PlayerPrefs.GetInt("currentSaveReciever"),transform.position.z);
                //kayıt burda bıraktığımız an çalışacak
                SaveRecieverDataBase();
            }
            Sayı = PlayerPrefs.GetInt("currentSaveReciever");
        }
        
        Debug.Log(PlayerPrefs.GetInt("currentSaveReciever"));
    }


    private void SaveRecieverDataBase()
    {
        //buraya eklenen antenleri veritabanına kayıtını yazacaksın.
    }
   
}
