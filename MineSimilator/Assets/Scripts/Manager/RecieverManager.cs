using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;
using System;
using Unity.VisualScripting;

public class RecieverManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> recievers;

    [SerializeField]
    public List<int> Saylist;

    [SerializeField]
    private GameObject recieverPref;

    [SerializeField]
    private GameObject recieverNamePanel;

    [SerializeField]
    private GameObject recieverNamePanelRM;

    [SerializeField]
    private TMP_InputField recieverNameInput;

    [SerializeField]
    private TMP_InputField recieverNameInputRM;

    [SerializeField]
    private GameObject recieversParent;

    [SerializeField]
    private RecieverManager self;

    [SerializeField]
    private ReadData readData;

    public class GameReciever
    {
        public int sayi { get; set; }
        public string ad { get; set; }
        public float posx { get; set; }
        public float posy { get; set; }
        public float posz { get; set; }
    }
    public class GameR
    {
        public List<GameReciever> gameRecievers= new List<GameReciever>();
    }

    
    public GameR gameR= new GameR();
    private Receiver tempReciever=new Receiver();
    private string currentRecieverName;
    private int addedReciever=0, oldcount;

    private void Awake() 
    {
        
        for (int i = 1; i <= PlayerPrefs.GetInt("currentSaveReciever"); i++)
        {
                var x = PlayerPrefs.GetFloat("Rposx" + i);
                var y = PlayerPrefs.GetFloat("Rposy" + i);
                var z = PlayerPrefs.GetFloat("Rposz" + i);

                var desPos = new Vector3(x, y, z);

                var newReciever = Instantiate(recieverPref, desPos, Quaternion.identity, recieversParent.transform);
                newReciever.GetComponent<AddedReciever>().RecieverManagg = self;
                newReciever.GetComponent<AddedReciever>().IsBreak = true;
                newReciever.GetComponent<AddedReciever>().gameObjects[1].GetComponent<TextMeshPro>().text = PlayerPrefs.GetString("RecieverName" + i);
                newReciever.GetComponent<AddedReciever>().gameObjects[0].GetComponent<TextMeshPro>().text = "Anten";

                newReciever.name = PlayerPrefs.GetString("RecieverName" + i);
                recievers.Add(newReciever);
                readData.signalList.Add(newReciever);

        }
        var jsonSaveFile = new MinerList() ;
        string convertJson = JsonUtility.ToJson(jsonSaveFile);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/RecieverList.json", convertJson);
    }

    //anten ekleme

    public void ProduceNewReciever()
    {
        if (recieverNameInput.text != string.Empty&& recievers.Exists(e => e.name == recieverNameInput.text)==false)
        {
            currentRecieverName = recieverNameInput.text;
            recieverNamePanel.SetActive(false);
            var newReciever = Instantiate(recieverPref);
            newReciever.GetComponent<AddedReciever>().RecieverManagg = self;

            PlayerPrefs.SetInt("currentSaveReciever", PlayerPrefs.GetInt("currentSaveReciever") + 1);

            int Sayý = PlayerPrefs.GetInt("currentSaveReciever");
            Saylist.Add(Sayý);

            newReciever.GetComponent<AddedReciever>().gameObjects[1].GetComponent<TextMeshPro>().text = currentRecieverName;
            newReciever.GetComponent<AddedReciever>().gameObjects[0].GetComponent<TextMeshPro>().text = "Anten";
            newReciever.name = currentRecieverName;
            recievers.Add(newReciever);


            PlayerPrefs.SetString("RecieverName" + PlayerPrefs.GetInt("currentSaveReciever"), currentRecieverName);

            readData.signalList.Add(newReciever);
            UpgradeJsonSave();

        }
        
    }

    public void AddNameReciever()
    {
        recieverNamePanel.SetActive(true);
    }

    public void AddNameRecieverClose()
    {
        recieverNamePanel.SetActive(false);
    }

    public void RemoveRecieverPanel()
    {
        recieverNamePanelRM.SetActive(true);
    }

    public void RemoveRecieverPanelClose()
    {
        recieverNamePanelRM.SetActive(false);
    }

    public void RemoveReciever()
    {
        var jsonSaveFile = readData.ReadJson();
        recieverNamePanelRM.SetActive(false);
        for (int i = 0; i < recievers.Count; i++)
        {
            if (recievers[i].name == recieverNameInputRM.text)
            {
                readData.signalList.Remove(recievers[i]);
                for (int j = 0; j < jsonSaveFile.minerRecieverList.Count; j++)
                {
                    for (int g = 0; g < jsonSaveFile.minerRecieverList[j].recieverList.Count; g++)
                    {
                        if (jsonSaveFile.minerRecieverList[j].recieverList[g].recieverName==recieverNameInputRM.text)
                        {
                            jsonSaveFile.minerRecieverList[j].recieverList.Remove(jsonSaveFile.minerRecieverList[j].recieverList[g]);
                        }
                    }
                    
                }
                var dst = recievers[i];
                recievers.Remove(recievers[i]);
                for (int j = 0; j < recievers.Count; j++)
                {
                    PlayerPrefs.SetFloat("Rposx" + (j + 1), recievers[j].transform.position.x);
                    PlayerPrefs.SetFloat("Rposy" + (j + 1), recievers[j].transform.position.y);
                    PlayerPrefs.SetFloat("Rposz" + (j + 1), recievers[j].transform.position.z);
                    PlayerPrefs.SetString("RecieverName" + (j + 1), recievers[j].name);
                }
                PlayerPrefs.SetInt("currentSaveReciever", PlayerPrefs.GetInt("currentSaveReciever") - 1);
                Destroy(dst);
                oldcount--;
            }
        }
        PlayerPrefs.Save();
        string convertJson = JsonUtility.ToJson(jsonSaveFile);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/RecieverList.json", convertJson);
    }

    public void _JsonSave(int maxTagCount)
    {

        MinerList miner=new MinerList();
        for (int i = 0; i < maxTagCount; i++)
        {
            RecieverList recieverList=new RecieverList();
        
            for (int j = 0; j < readData.signalList.Count; j++)
            {
                recieverList.recieverList.Add(tempReciever);
                
            }

            miner.minerRecieverList.Add(recieverList);
            
        }
        oldcount = readData.signalList.Count;
        string convertJson=JsonUtility.ToJson(miner);
        System.IO.File.WriteAllText(Application.persistentDataPath+"/RecieverList.json",convertJson);
    }

    public void UpgradeJsonSave()
    {

        MinerList miner = readData.ReadJson();
        for (int i = 0; i < miner.minerRecieverList.Count; i++)
        {
            RecieverList recieverList = new RecieverList();

            for (int j = oldcount; j < readData.signalList.Count; j++)
            {
                miner.minerRecieverList[i].recieverList.Add(tempReciever);
            }
        }
        oldcount= readData.signalList.Count;
        string convertJson = JsonUtility.ToJson(miner);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/RecieverList.json", convertJson);
    }

    public void UpdateJsonSave(List<Receiver> jsonSaveFile2,int newSignalCount, int listNumber,int minerNumber)
    {
          var jsonSaveFile=readData.ReadJson();
         
          jsonSaveFile.minerRecieverList[minerNumber].recieverList[listNumber].signalCount=newSignalCount;
          jsonSaveFile.minerRecieverList[minerNumber].recieverList[listNumber].recieverName=readData.signalList[listNumber].name;
        //jsonSaveFile[listNumber].signalCount=newSignalCount;

        string convertJson=JsonUtility.ToJson(jsonSaveFile);
         System.IO.File.WriteAllText(Application.persistentDataPath+"/RecieverList.json",convertJson);

    }




}
