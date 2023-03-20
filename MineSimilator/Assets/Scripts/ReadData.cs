using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System.Linq;
using Unity.VisualScripting;
using System.Xml.Linq;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ReadData : MonoBehaviour
{
    
   
    public List<GameObject> signalList=new List<GameObject>();
    //public List<Receiver> tamplist = new List<Receiver>();

    public PathCreator path;

    private float bigSignal=0,secondBigSignal=0,firstSignal=0,secondSignal=0;
    private  int countBig,countSmall, bigSignalNumber, secondBigSignalNumber;
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

    public void MoveBySignal(GameObject miner, List<string> mineRecieverNameList, List<int> minerReceiverCountList)
    {

        GetTwoBigRecieverSignal(minerReceiverCountList);

           firstPos = signalList.Find(i => i.name == mineRecieverNameList[bigSignalNumber]);
           secondPos = signalList.Find(i => i.name == mineRecieverNameList[secondBigSignalNumber]);

        //if (countBig > countSmall)
        //{
        //    firstPos = secondBigPos;
        //    secondPos = bigPos;
        //}
        //else
        //{
        //    firstPos = bigPos;
        //    secondPos = secondBigPos;
        //}

        //var A1 = signalList.Find(i => i.name == BName);
        //var A2 = signalList.Find(i => i.name == SBName);
        //float x1 = A1.transform.position.x;
        //float x2 = A2.transform.position.x;
        //float y1 = A1.transform.position.y;
        //float y2 = A2.transform.position.y;
        //float z1 = A1.transform.position.z;
        //float z2 = A2.transform.position.z;
        //if (x1 < x2)
        //{
        //    firstPosX = x1;
        //    firstSignal = bigSignal;
        //    secondPosX = x2;
        //    secondSignal = secondBigSignal;
        //}
        //else if (x1 > x2)
        //{
        //    firstPosX = x2;
        //    firstSignal = secondBigSignal;
        //    secondPosX = x1;
        //    secondSignal = bigSignal;
        //}
        //if (y1 < y2)
        //{
        //    firstPosY = y1;
        //    firstSignal = bigSignal;
        //    secondPosY = y2;
        //    secondSignal = secondBigSignal;
        //}
        //else if (y1 > y2)
        //{
        //    firstPosY = y2;
        //    firstSignal = secondBigSignal;
        //    secondPosY = y1;
        //    secondSignal = bigSignal;
        //}
        //if (z1 < z2)
        //{
        //    firstPosZ = z1;
        //    firstSignal = bigSignal;
        //    secondPosZ = z2;
        //    secondSignal = secondBigSignal;
        //}
        //else if (z1 > z2)
        //{
        //    firstPosZ = z2;
        //    firstSignal = secondBigSignal;
        //    secondPosZ = z1;
        //    secondSignal = bigSignal;
        //}

        Debug.Log(bigSignal);
        Debug.Log(secondBigSignal);
        Debug.Log(bigPos);
        Debug.Log(secondBigPos);

        //var desPosZ=((Math.Abs(firstPos.transform.position.z-secondPos.transform.position.z))*(secondSignal/(firstSignal+secondSignal))+firstPos.transform.position.z);
        var desPos = new Vector3(Math.Abs(firstPos.transform.position.x - secondPos.transform.position.x) * (secondSignal / (firstSignal + secondSignal)) + firstPos.transform.position.x
        , Math.Abs(firstPos.transform.position.y - secondPos.transform.position.y) * (secondSignal / (firstSignal + secondSignal)) + firstPos.transform.position.y,
        Math.Abs(firstPos.transform.position.z - secondPos.transform.position.z) * (secondSignal / (firstSignal + secondSignal)) + firstPos.transform.position.z
        );
        Debug.Log(desPos);
        desPos = path.path.GetClosestPointOnPath(desPos);
        Debug.Log(desPos);
        LeanTween.move(miner, desPos, .5f).setEaseInCubic().setOnComplete(() =>
        {
            bigSignal = 0;
            secondBigSignal = 0;
        });


    }


    private void GetTwoBigRecieverSignal(List<int> minerReceiverList)
    {

        // var recieverList=ReadJson().minerRecieverList[minerNumber].recieverList;
        var tempSignal = minerReceiverList[0];

        for (int i = 0; i < minerReceiverList.Count; i++)
        {
            var tempName = minerReceiverList[i];

            if (minerReceiverList[i] > bigSignal)
            {
                bigSignal = minerReceiverList[i];
                bigSignalNumber = tempName;
                tempSignal = minerReceiverList[i];
                firstSignal = i;

            }
        }
        minerReceiverList.Remove(tempSignal);
        for (int i = 0; i < minerReceiverList.Count; i++)
        {
            var tempName = minerReceiverList[i];

            if (minerReceiverList[i] > secondBigSignal)
            {
                secondBigSignal = minerReceiverList[i];
                secondBigSignalNumber = tempName;
                secondSignal = i;

            }
        }
        minerReceiverList.Add(tempSignal);

        if (firstSignal > secondSignal)
        {
            firstSignal = secondBigSignal;
            secondSignal = bigSignal;
        }
        else
        {
            firstSignal = bigSignal;
            secondSignal = secondBigSignal;
        }

        Debug.Log(bigSignal);
        Debug.Log(secondBigSignal);

    }

}