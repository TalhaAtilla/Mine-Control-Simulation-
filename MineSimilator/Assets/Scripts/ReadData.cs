using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System.Linq;
using Unity.VisualScripting;

public class ReadData : MonoBehaviour
{
    
   
    public List<GameObject> signalList=new List<GameObject>();
    //public List<Receiver> tamplist = new List<Receiver>();

    public PathCreator path;

    private float bigSignal=0,secondBigSignal=0,firstSignal=0,secondSignal=0;
    private  int countBig,countSmall;
    private string bigSignalName,secondBigSignalName;
    private GameObject firstPos,secondPos,secondBigPos,bigPos;

    private void Start()
    {
        Debug.Log(signalList[2]);
    }

    
    //private MinerList ReadJson()
    //{
    //    string JsonData = System.IO.File.ReadAllText(Application.persistentDataPath + "/RecieverList.json");
    //    MinerList minerList = JsonUtility. FromJson<MinerList>(JsonData);
    //    return minerList;
    //}

    //public void MoveBySignal(GameObject miner, List<int> minerReceiverCountList,)
    //{
    //    TagManager.Instance.UpdateMinersRecieverListCount();    

    //    GetTwoBigRecieverSignal(minerReceiverList);

    //    for (int i = 0; i < signalList.Count; i++)
    //    {
    //        if(signalList[i].name==bigSignalName)
    //        {
    //            bigPos=signalList[i];
    //            countBig=i;
    //        }
    //        else if(signalList[i].name==secondBigSignalName)
    //        {
    //            secondBigPos=signalList[i];
    //            countSmall=i;
    //        }
    //    }
        
    //    if(countBig>countSmall)
    //    {
    //        firstPos=secondBigPos;
    //        secondPos=bigPos;
    //    }
    //    else
    //    {
    //        firstPos=bigPos;
    //        secondPos=secondBigPos;
    //    }

    //    Debug.Log(bigSignal); 
    //    Debug.Log(secondBigSignal);
    //    Debug.Log(bigPos);
    //    Debug.Log(secondBigPos);

    //    //var desPosZ=((Math.Abs(firstPos.transform.position.z-secondPos.transform.position.z))*(secondSignal/(firstSignal+secondSignal))+firstPos.transform.position.z);
    //    var desPos=new Vector3(Math.Abs(firstPos.transform.position.x-secondPos.transform.position.x)*(secondSignal/(firstSignal+secondSignal))+firstPos.transform.position.x
    //    ,Math.Abs(firstPos.transform.position.y-secondPos.transform.position.y)*(secondSignal/(firstSignal+secondSignal))+firstPos.transform.position.y,
    //    Math.Abs(firstPos.transform.position.z-secondPos.transform.position.z)*(secondSignal/(firstSignal+secondSignal))+firstPos.transform.position.z
    //    );
    //    Debug.Log(desPos);
    //    desPos=path.path.GetClosestPointOnPath(desPos);
    //    Debug.Log(desPos);
    //    LeanTween.move(miner,desPos,.5f).setEaseInCubic().setOnComplete(()=>
    //    {
    //        bigSignal=0;
    //        secondBigSignal=0;
    //    });
        

   // }


    //private void GetTwoBigRecieverSignal(List<Receiver> minerReceiverList)
    //{
        
    //    // var recieverList=ReadJson().minerRecieverList[minerNumber].recieverList;
    //    var tempSignal=minerReceiverList[0];

    //    for (int i = 0; i < minerReceiverList.Count ; i++)
    //    {
    //        var tempName=minerReceiverList[i].recieverName;
            
    //        if(minerReceiverList[i].signalCount>bigSignal)
    //        {
    //            bigSignal=minerReceiverList[i].signalCount;
    //            bigSignalName=tempName;
    //            tempSignal=minerReceiverList[i];
    //            firstSignal=i;
                
    //        }
    //    }
    //    minerReceiverList.Remove(tempSignal);
    //    for (int i = 0; i < minerReceiverList.Count ; i++)
    //    {
    //        var tempName=minerReceiverList[i].recieverName;
            
    //        if(minerReceiverList[i].signalCount>secondBigSignal)
    //        {
    //            secondBigSignal=minerReceiverList[i].signalCount;
    //            secondBigSignalName=tempName;
    //            secondSignal=i;
                
    //        }
    //    }
    //    minerReceiverList.Add(tempSignal);

    //    if(firstSignal>secondSignal)
    //    {
    //        firstSignal=secondBigSignal;
    //        secondSignal=bigSignal;
    //    }
    //    else
    //    {
    //        firstSignal=bigSignal;
    //        secondSignal=secondBigSignal;
    //    }

    //    Debug.Log(bigSignal);
    //    Debug.Log(secondBigSignal);
        
    //}

}