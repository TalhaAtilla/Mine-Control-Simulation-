using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class StationManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> stations;

    [SerializeField]
    private GameObject StationPref;

    [SerializeField]
    private GameObject StationNamePanel;

    [SerializeField]
    private GameObject StationRemovePanel;

    [SerializeField]
    private TMP_InputField StationNameInput;

    [SerializeField]
    private TMP_InputField StationNameInputRM;

    [SerializeField]
    private GameObject StationsParent;

    [SerializeField]
    private UIManager UI;

    private Receiver tempStation = new Receiver();
    private string currentStationName;
    private int addedStation = 0, oldcount;

    private void Awake()
    {

        tempStation.recieverName = "A";
        tempStation.signalCount = 0;
        StationNamePanel.SetActive(false);

        for (int i = 1; i <= PlayerPrefs.GetInt("currentSaveStation"); i++)
        {
            var x = PlayerPrefs.GetFloat("posx" + i);
            var y = PlayerPrefs.GetFloat("posy" + i);
            var z = PlayerPrefs.GetFloat("posz" + i);
            var desPos = new Vector3(x, y, z);

            var newStation = Instantiate(StationPref, desPos, Quaternion.identity, StationsParent.transform);
            newStation.GetComponent<AddedStation>().IsBreak = true;
            for (int j = 1; j < 8; j++)
            {
                UI.stationTexts[j + (8 * (Convert.ToInt16(PlayerPrefs.GetString("StationName" + i)) - 1))] = newStation.GetComponent<AddedStation>().gameObjects[j - 1].GetComponent<TextMeshPro>();
            }
            newStation.GetComponent<AddedStation>().gameObjects[8].GetComponent<TextMeshPro>().text = "Ýstasyon "+PlayerPrefs.GetString("StationName" + i);
            stations.Add(newStation);
            newStation.name = PlayerPrefs.GetString("StationName" + i);

        }
    }

    public void ProduceNewStation()
    {
        currentStationName = StationNameInput.text;
        StationNamePanel.SetActive(false);
        var newStation = Instantiate(StationPref);
        PlayerPrefs.SetInt("currentSaveStation", PlayerPrefs.GetInt("currentSaveStation") + 1);

        for (int i = 1; i < 9 ; i++) 
        { 
            UI.stationTexts[i + ( 8 * (Convert.ToInt16(currentStationName) -1 ) ) ] = newStation.GetComponent<AddedStation>().gameObjects[i-1].GetComponent<TextMeshPro>();
        }

        newStation.GetComponent<AddedStation>().gameObjects[8].GetComponent<TextMeshPro>().text = "Ýstasyon "+currentStationName;
        newStation.name = currentStationName;
        stations.Add(newStation);
        PlayerPrefs.SetString("StationName" + PlayerPrefs.GetInt("currentSaveStation"), currentStationName);

    }

    public void AddNameStation()
    {
        StationNamePanel.SetActive(true);
    }

    public void CloseAddPanel()
    {
        StationNamePanel.SetActive(false);
    }

    public void CloseRemovePanel() 
    {
        StationRemovePanel.SetActive(false);
    }

    public void RemeoveStation1() 
    {
        StationRemovePanel.SetActive(true);
    }

    public void RemeoveStation2()
    {
 
        for (int i = 0;i< stations.Count;i++)
        {
            if (stations[i].name==StationNameInputRM.text)
            {
                stations[i].SetActive(false);
            }
        }

        StationRemovePanel.SetActive(false);
    }

}
