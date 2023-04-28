using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using static RecieverManager;

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
    private StationManager self;

    [SerializeField]
    private UIManager UI;

    public class GameStation
    {
        public int sayi { get; set; }
        public string ad { get; set; }
        public float posx { get; set; }
        public float posy { get; set; }
        public float posz { get; set; }
    }

    public List<GameStation> gameStations = new List<GameStation>();

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
            var x = PlayerPrefs.GetFloat("Sposx" + i);
            var y = PlayerPrefs.GetFloat("Sposy" + i);
            var z = PlayerPrefs.GetFloat("Sposz" + i);

            var desPos = new Vector3(x, y, z);

            var newStation = Instantiate(StationPref, desPos, Quaternion.identity, StationsParent.transform);
            newStation.GetComponent<AddedStation>().StationManagg = self;
            newStation.GetComponent<AddedStation>().IsBreak = true;
            for (int j = 1; j < 8; j++)
            {
                UI.stationTexts[j + (8 * PlayerPrefs.GetInt("currentSaveStation") - 1)] = newStation.GetComponent<AddedStation>().gameObjects[j - 1].GetComponent<TextMeshPro>();
            }
            newStation.GetComponent<AddedStation>().gameObjects[8].GetComponent<TextMeshPro>().text = "Ýstasyon "+ PlayerPrefs.GetString("StationName" + i);
            newStation.name = PlayerPrefs.GetString("StationName" + i);
            stations.Add(newStation);

        }
    }

    public void ProduceNewStation()
    {
        if (StationNameInput.text != string.Empty && stations.Exists(e => e.name == StationNameInput.text) == false)
        {
            currentStationName = StationNameInput.text;
            StationNamePanel.SetActive(false);
            var newStation = Instantiate(StationPref);
            newStation.GetComponent<AddedStation>().StationManagg = self;
            PlayerPrefs.SetInt("currentSaveStation", PlayerPrefs.GetInt("currentSaveStation") + 1);
            if (Convert.ToInt16(currentStationName) < 9)
            {
                for (int i = 1; i < 9; i++)
                {
                    UI.stationTexts[i + (8 * (Convert.ToInt16(currentStationName) - 1))] = newStation.GetComponent<AddedStation>().gameObjects[i - 1].GetComponent<TextMeshPro>();
                }
            }
            newStation.GetComponent<AddedStation>().gameObjects[8].GetComponent<TextMeshPro>().text = "Ýstasyon " + currentStationName;
            newStation.name = currentStationName;
            stations.Add(newStation);


            PlayerPrefs.SetString("StationName" + PlayerPrefs.GetInt("currentSaveStation"), currentStationName);
        }

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
                var dst = stations[i];
                stations.Remove(stations[i]);
                
                for (int j = 0; j < stations.Count; j++)
                {
                    PlayerPrefs.SetFloat("Sposx" + (j + 1), stations[j].transform.position.x);
                    PlayerPrefs.SetFloat("Sposy" + (j + 1), stations[j].transform.position.y);
                    PlayerPrefs.SetFloat("Sposz" + (j + 1), stations[j].transform.position.z);
                    PlayerPrefs.SetString("StationName" + (j + 1), stations[j].name);
                }
                Destroy(dst);
                PlayerPrefs.SetInt("currentSaveStation", PlayerPrefs.GetInt("currentSaveStation") - 1);
                PlayerPrefs.Save();
            }
        }
        


        StationRemovePanel.SetActive(false);
    }


}
