using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class TagManager : Singleton<TagManager>
{
    [SerializeField]
    private GameObject tagPref;

    [SerializeField]
    private Vector3 startPos;

    [SerializeField]
    private int maxTagCount;

    [SerializeField]
    private ReadData readData;

    public List<GameObject> tagList=new List<GameObject>();
    private string[] data;
    
    void Start()
    {
            


        for (int i = 0; i < maxTagCount; i++)
        {
           var newTag= Instantiate(tagPref,startPos,Quaternion.identity);
            newTag.GetComponent<Miner>().readData = readData;
            tagList.Add(newTag);
            for (int x = 0; x < readData.signalList.Count; x++)
            {
                newTag.GetComponent<Miner>().mineRecieverNameList.Add(readData.signalList[x].name);
                newTag.GetComponent<Miner>().mineRecieverCountList.Add(0);
            }
            newTag.name = i.ToString();
            newTag.SetActive(false);
            Debug.Log(newTag);
        }
    }

    void Update()
    {

        
    }

    private void AddTag(int number)
    {
        for (int i = maxTagCount; i < number+1; i++)
        {
            
            var newTag = Instantiate(tagPref, startPos, Quaternion.identity);
            newTag.GetComponent<Miner>().readData = readData;
            for (int x = 0; x < readData.signalList.Count; x++)
            {
                newTag.GetComponent<Miner>().mineRecieverNameList.Add(readData.signalList[x].name);
                newTag.GetComponent<Miner>().mineRecieverCountList.Add(0);
            }
            newTag.name = i.ToString();
            newTag.SetActive(false);

            Debug.Log(newTag);
        }
        maxTagCount= number+1;
    }


    public void UpdateMinersRecieverListCount()
    {
        StartCoroutine(GetRequest("http://127.0.0.1:8080/sql/sql6.php"));
        for (int j = 0; j < data.Count(); j++)
        {
            string[] fin = data[j].Split("/");
            Debug.Log(fin[0]);
            for (int i = 0; i < tagList.Count; i++)
            {
                

                if (tagList[i].name == fin[0])
                {
                tagList[i].GetComponent<Miner>().mineRecieverNameList.Clear();
                tagList[i].GetComponent<Miner>().mineRecieverCountList.Clear();
                for (int x = 0; x < readData.signalList.Count; x++)
                {
                    tagList[i].GetComponent<Miner>().mineRecieverNameList.Add(readData.signalList[x].name);
                    tagList[i].GetComponent<Miner>().mineRecieverCountList.Add(0);
                }


                var mineRecieverList = tagList[i].GetComponent<Miner>().mineRecieverNameList;
                var mineRecieverCounts = tagList[i].GetComponent<Miner>().mineRecieverCountList;

                    for (int x = 0; x < mineRecieverList.Count; x++)
                    {
                        if (mineRecieverList[x] == fin[3])
                        {
                           mineRecieverCounts[x] = Convert.ToInt32(fin[2]);
                        }
                    }

                tagList[i].SetActive(true);
            }
            else if (Convert.ToInt16(fin[0])>maxTagCount)
            {
                AddTag(Convert.ToInt16(fin[0]));
            }
            }              
        }
            
    }

    
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);

                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string raw = webRequest.downloadHandler.text;
                    Debug.Log(raw);
                    //example raw= 35/-13,33/-32,
                    data = raw.Split(",");
                    break;
            }
        }
    }

}
