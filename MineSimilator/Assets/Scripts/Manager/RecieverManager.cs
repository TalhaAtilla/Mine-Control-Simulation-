using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecieverManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> recievers;

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
    private ReadData readData;

    private Receiver tempReciever=new Receiver();
    private string currentRecieverName;
    private int addedReciever=0, oldcount;

    private void Awake() 
    {
    
        tempReciever.recieverName="A";
        tempReciever.signalCount=0;
        recieverNamePanel.SetActive(false);
        
        for (int i = 1; i <= PlayerPrefs.GetInt("currentSaveReciever"); i++)
        {
            var x=PlayerPrefs.GetFloat("posx"+i);
            var y=PlayerPrefs.GetFloat("posy"+i);
            var z=PlayerPrefs.GetFloat("posz"+i);
            var desPos= new Vector3(x,y,z);

           var newReciever= Instantiate(recieverPref,desPos,Quaternion.identity,recieversParent.transform);
           newReciever.GetComponent<AddedReciever>().IsBreak=true;
            newReciever.GetComponent<AddedReciever>().gameObjects[1].GetComponent<TextMeshPro>().text = PlayerPrefs.GetString("RecieverName" + i);
            newReciever.GetComponent<AddedReciever>().gameObjects[0].GetComponent<TextMeshPro>().text = "Anten";

            newReciever.name=PlayerPrefs.GetString("RecieverName"+i);
            recievers.Add(newReciever);
           readData.signalList.Add(newReciever);
          
        }
    }

    //anten ekleme

    public void ProduceNewReciever()
    {
        currentRecieverName=recieverNameInput.text;
        recieverNamePanel.SetActive(false);
        var newReciever=Instantiate(recieverPref);
        PlayerPrefs.SetInt("currentSaveReciever",PlayerPrefs.GetInt("currentSaveReciever")+1);
        newReciever.GetComponent<AddedReciever>().gameObjects[1].GetComponent<TextMeshPro>().text =currentRecieverName;
        newReciever.GetComponent<AddedReciever>().gameObjects[0].GetComponent<TextMeshPro>().text ="Anten";
        newReciever.name=currentRecieverName;
        recievers.Add(newReciever);

        PlayerPrefs.SetString("RecieverName"+PlayerPrefs.GetInt("currentSaveReciever"),currentRecieverName);
        readData.signalList.Add(newReciever);
        UpgradeJsonSave();
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
        recieverNamePanel.SetActive(false);
        for (int i = 0; i < recievers.Count; i++)
        {
            if (recievers[i].name == recieverNameInputRM.text)
            {
                recievers[i].SetActive(false);
                readData.signalList.Remove(recievers[i]);
                for (int j = 0; j < jsonSaveFile.minerRecieverList.Count; j++)
                {
                    for (int g = 0; g < jsonSaveFile.minerRecieverList[j].recieverList.Count; g++)
                    {
                        if (jsonSaveFile.minerRecieverList[j].recieverList[g].recieverName==recieverNameInputRM.text)
                        {
                            jsonSaveFile.minerRecieverList[j].recieverList[g].signalCount=0;
                        }
                    }
                    
                }
                
            }
        }
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
