using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using System;
using UnityEngine.Networking;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Unity.VisualScripting;
using System.Text.RegularExpressions;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainProgram;

    [SerializeField]
    private GameObject loginPanel;

    [SerializeField]
    private GameObject showValuePanel;

    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private List<TextMeshProUGUI> gridTexts;

    [SerializeField]
    public List<TextMeshPro> stationTexts;

    [SerializeField]
    private GameObject camHolder;

    [SerializeField]
    private double timer;

    [SerializeField]
    private bool timerIsActive = true;

    private List<string> textList = new List<string>();

    private bool isShowPanelActive=false;

    public class data : IComparable<data>
    {
        public int id;
        public string ad;
        public int deger;

        public int CompareTo(data other)
        {
            if (this.id > other.id)
            {
                return 1;
            }else if (this.id < other.id)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    private List<data> dataList = new List<data>();

    // Start is called before the first frame update
    void Awake()
    {
        CloseAllPanel();
        loginPanel.SetActive(true);

        

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GetRequest("http://127.0.0.1:8080/sql/sql4.php"));
        FillGrid();
    }


    public void OpenShowValue()
    {
        var posShowPanelPos = showValuePanel.transform.position;
        var mainMenuPanelPos = mainMenuPanel.transform.position;
        if (!isShowPanelActive)
        {
            showValuePanel.SetActive(true);
            showValuePanel.LeanMoveLocalX(551f, 1f);
            mainMenuPanel.LeanMoveLocalX(75f, 1f);
            isShowPanelActive = true;
        }
        else
        {
            mainMenuPanel.LeanMoveLocalX(mainMenuPanelPos.x - 146, 1f);
            showValuePanel.LeanMoveX(posShowPanelPos.x + 823, 1f).setOnComplete(() =>
            {
                showValuePanel.SetActive(false);

            });
            isShowPanelActive = false;

        }

    }

    private void WriteGridTexts(float timer)
    {
        
        timer-=Time.deltaTime;
        if(timer<=0)
        {
            StartCoroutine(GetRequest("http://localhost/sql/sql4.php"));
            FillGrid();
            //bu methodu update de timera 4 sn vererek çalıştır. WriteGridTexts(4f)
            //aynı zamanda yeni yaptığım uı ekranını butonla çalıştırıyorsun. Sen Editörden Setactivini aç bi incele.
            //Yeni eklenen kısımda ise elemanları göreceksin, SQL den çekeceğin değerleri burada gridTexts[0,1,3].text= değer; şeklinde yazdırabilirsin.
            //sonrasında yeni aldığımız stationtext[i]=gridtexts[i] şeklinde eşitleyeceksin.
        }
    }

    private void CloseAllPanel()
    {
        showValuePanel.SetActive(false);
        mainProgram.SetActive(false);
        mainMenuPanel.SetActive(false);
    }

    public void Login()
    {
        //buraya veritabanı kontrolü eklenecek;
        loginPanel.SetActive(false);
        mainProgram.SetActive(true);
        mainMenuPanel.SetActive(true);
        camHolder.transform.position=new Vector3(1.5199f,30.520f,-16.65f);

    }
    void LateUpdate()
    {
        if(timerIsActive)
        {
            timer-=Time.deltaTime;
            if(timer<0)
            {
                TagManager.Instance.UpdateMinersRecieverListCount();

                timerIsActive = false;


            }
            TagManager.Instance.UpdateMinersRecieverListCount();


        }
        FillTables();
        

    }

    public void Register()
    {
        //buraya veritabanına kayıt koyulacak;
    }
    private void FillGrid()
    {
        int count = 0;
        int nowid = dataList[0].id;
        for (int i = 0; i < dataList.Count; i++)
        {
            if (nowid > dataList[i].id)
            {
                nowid = dataList[i].id;
            }
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            while (nowid-1 < dataList[i].id && dataList[i].id < (nowid + 3))
            {
                string[] laz = dataList[i].deger.ToString().Select(x => x.ToString()).ToArray();
                int lenn = laz.Length;
                UnityEngine.Debug.Log(lenn);
                string grd1 = null;
                string grd2 = null;
                if (dataList[i].ad.ToLower() == "ch4")
                {

                    if (dataList[i].deger >= 200)//ch4
                    {
                        if (stationTexts[(count * 8) + 1] != null)
                        {
                            stationTexts[(count * 8) + 1].GetComponent<TextMeshPro>().color = Color.red;
                        }

                        gridTexts[(count * 8) + 1].GetComponent<TextMeshProUGUI>().color = Color.red;
                    }
                    else if (dataList[i].deger >= 150)
                    {
                        if (stationTexts[(count * 8) + 1] != null)
                        {
                            stationTexts[(count * 8) + 1].GetComponent<TextMeshPro>().color = Color.yellow;
                        }

                        gridTexts[(count * 8) + 1].GetComponent<TextMeshProUGUI>().color = Color.yellow;

                    }
                    else
                    {
                        if (stationTexts[(count * 8) + 1] != null)
                        {
                            stationTexts[(count * 8) + 1].GetComponent<TextMeshPro>().color = Color.white;
                        }

                        gridTexts[(count * 8) + 1].GetComponent<TextMeshProUGUI>().color = Color.black;

                    }

                    if (lenn >= 3)
                    {
                        for (int x = lenn - 2; x < lenn; x++)
                        {
                            grd1 += laz[x];
                        }
                        for (int x = 0; x < lenn - 2; x++)
                        {
                            grd2 += laz[x];
                        }
                        gridTexts[(count * 8) + 1].GetComponent<TextMeshProUGUI>().text = grd2 + "." + grd1 + " ppm";
                    }
                    else if (lenn == 2)
                    {
                        gridTexts[(count * 8) + 1].GetComponent<TextMeshProUGUI>().text = "0" + "." + dataList[i].deger.ToString() + " ppm";
                    }
                    else
                    {
                        gridTexts[(count * 8) + 1].GetComponent<TextMeshProUGUI>().text = "0" + "." + "0" + dataList[i].deger.ToString() + " ppm";
                    }



                }
                else if (dataList[i].ad.ToLower() == "co")
                {

                    if (dataList[i].deger >= 30)//co
                    {
                        if (stationTexts[(count * 8) + 2] != null)
                        {
                            stationTexts[(count * 8) + 2].GetComponent<TextMeshPro>().color = Color.red;
                        }
                        
                        gridTexts[(count * 8) + 2].GetComponent<TextMeshProUGUI>().color = Color.red;

                    }
                    else if (dataList[i].deger >= 20)
                    {
                        if (stationTexts[(count * 8) + 2] != null)
                        {
                            stationTexts[(count * 8) + 2].GetComponent<TextMeshPro>().color = Color.yellow;
                        }
                        
                        gridTexts[(count * 8) + 2].GetComponent<TextMeshProUGUI>().color = Color.yellow;

                    }
                    else
                    {
                        if (stationTexts[(count * 8) + 2] != null)
                        {
                            stationTexts[(count * 8) + 2].GetComponent<TextMeshPro>().color = Color.white;
                        }
                        
                        gridTexts[(count * 8) + 2].GetComponent<TextMeshProUGUI>().color = Color.black;

                    }

                    grd1 = dataList[i].deger.ToString();

                    gridTexts[(count * 8) + 2].GetComponent<TextMeshProUGUI>().text = grd1 + " ppm";
                }
                else if (dataList[i].ad.ToLower() == "o2")
                {

                    if (dataList[i].deger <= 190)//o2
                    {
                        if (stationTexts[(count * 8) + 3] != null)
                        {
                            stationTexts[(count * 8) + 3].GetComponent<TextMeshPro>().color = Color.red;
                        }
                        
                        gridTexts[(count * 8) + 3].GetComponent<TextMeshProUGUI>().color = Color.red;

                    }
                    else if (dataList[i].deger >= 230)
                    {
                        if (stationTexts[(count * 8) + 3] != null)
                        {
                            stationTexts[(count * 8) + 3].GetComponent<TextMeshPro>().color = Color.red;
                        }
                        
                        gridTexts[(count * 8) + 3].GetComponent<TextMeshProUGUI>().color = Color.red;

                    }
                    else
                    {
                        if (stationTexts[(count * 8) + 3] != null)
                        {
                            stationTexts[(count * 8) + 3].GetComponent<TextMeshPro>().color = Color.white;
                        }
                        
                        gridTexts[(count * 8) + 3].GetComponent<TextMeshProUGUI>().color = Color.black;

                    }

                    if (lenn >= 3)
                    {
                        for (int x = lenn - 1; x < lenn; x++)
                        {
                            grd1 += laz[x];
                        }
                        for (int x = 0; x < lenn - 1; x++)
                        {
                            grd2 += laz[x];
                        }
                        gridTexts[(count * 8) + 3].GetComponent<TextMeshProUGUI>().text = grd2 + "." + grd1 + " %";
                    }
                    else if (lenn == 2)
                    {
                        gridTexts[(count * 8) + 3].GetComponent<TextMeshProUGUI>().text = laz[0] + "." + laz[1] + " %";
                    }
                    else
                    {
                        gridTexts[(count * 8) + 3].GetComponent<TextMeshProUGUI>().text = "0" + "." + dataList[i].deger.ToString() + " %";
                    }
                }
                else if (dataList[i].ad.ToLower() == "h2s")
                {

                    if (dataList[i].deger >= 20)//h2s
                    {
                        if (stationTexts[(count * 8) + 4] != null)
                        {
                            stationTexts[(count * 8) + 4].GetComponent<TextMeshPro>().color = Color.red;
                        }
                        
                        gridTexts[(count * 8) + 4].GetComponent<TextMeshProUGUI>().color = Color.red;

                    }
                    else if (dataList[i].deger >= 10)
                    {
                        if (stationTexts[(count * 8) + 4] != null)
                        {
                            stationTexts[(count * 8) + 4].GetComponent<TextMeshPro>().color = Color.yellow;
                        }
                        
                        gridTexts[(count * 8) + 4].GetComponent<TextMeshProUGUI>().color = Color.yellow;

                    }
                    else
                    {
                        if (stationTexts[(count * 8) + 4] != null)
                        {
                            stationTexts[(count * 8) + 4].GetComponent<TextMeshPro>().color = Color.white;
                        }
                        
                        gridTexts[(count * 8) + 4].GetComponent<TextMeshProUGUI>().color = Color.black;

                    }

                    grd1 = dataList[i].deger.ToString();

                    gridTexts[(count * 8) + 4].GetComponent<TextMeshProUGUI>().text = grd1 + " ppm";
                }
                else if (dataList[i].ad.ToLower() == "hh")
                {

                    if (dataList[i].deger >= 100)//hh
                    {
                        if (stationTexts[(count * 8) + 5] != null)
                        {
                            stationTexts[(count * 8) + 5].GetComponent<TextMeshPro>().color = Color.red;
                        }
                        
                        gridTexts[(count * 8) + 5].GetComponent<TextMeshProUGUI>().color = Color.red;

                    }
                    else if (dataList[i].deger >= 50)
                    {
                        if (stationTexts[(count * 8) + 5] != null)
                        {
                            stationTexts[(count * 8) + 5].GetComponent<TextMeshPro>().color = Color.yellow;
                        }
                        
                        gridTexts[(count * 8) + 5].GetComponent<TextMeshProUGUI>().color = Color.yellow;

                    }
                    else
                    {
                        if (stationTexts[(count * 8) + 5] != null)
                        {
                            stationTexts[(count * 8) + 5].GetComponent<TextMeshPro>().color = Color.white;
                        }
                        
                        gridTexts[(count * 8) + 5].GetComponent<TextMeshProUGUI>().color = Color.black;

                    }

                    if (lenn >= 3)
                    {
                        for (int x = lenn - 1; x < lenn; x++)
                        {
                            grd1 += laz[x];
                        }
                        for (int x = 0; x < lenn - 1; x++)
                        {
                            grd2 += laz[x];
                        }
                        gridTexts[(count * 8) + 5].GetComponent<TextMeshProUGUI>().text = grd2 + "." + grd1 + " m/sn";
                    }
                    else if (lenn == 2)
                    {
                        gridTexts[(count * 8) + 5].GetComponent<TextMeshProUGUI>().text = laz[0] + "." + laz[1] + " m/sn";
                    }
                    else
                    {
                        gridTexts[(count * 8) + 5].GetComponent<TextMeshProUGUI>().text = "0" + "." + dataList[i].deger.ToString() + " m/sn";
                    }

                }
                else if (dataList[i].ad.ToLower() == "ısı")
                {

                    if (dataList[i].deger >= 400)//ısı
                    {
                        if (stationTexts[(count * 8) + 6] != null)
                        {
                            stationTexts[(count * 8) + 6].GetComponent<TextMeshPro>().color = Color.red;
                        }
                        
                        gridTexts[(count * 8) + 6].GetComponent<TextMeshProUGUI>().color = Color.red;

                    }
                    else if (dataList[i].deger >= 300)
                    {
                        if (stationTexts[(count * 8) + 6] != null)
                        {
                            stationTexts[(count * 8) + 6].GetComponent<TextMeshPro>().color = Color.yellow;
                        }
                        
                        gridTexts[(count * 8) + 6].GetComponent<TextMeshProUGUI>().color = Color.yellow;

                    }
                    else
                    {
                        if (stationTexts[(count * 8) + 6] != null)
                        {
                            stationTexts[(count * 8) + 6].GetComponent<TextMeshPro>().color = Color.white;
                        }
                        
                        gridTexts[(count * 8) + 6].GetComponent<TextMeshProUGUI>().color = Color.black;

                    }


                    if (lenn >= 3)
                    {
                        for (int x = lenn - 1; x < lenn; x++)
                        {
                            grd1 += laz[x];
                        }
                        for (int x = 0; x < lenn - 1; x++)
                        {
                            grd2 += laz[x];
                        }
                        gridTexts[(count * 8) + 6].GetComponent<TextMeshProUGUI>().text = grd2 + "." + grd1 + " °C";
                    }
                    else if (lenn == 2)
                    {
                        gridTexts[(count * 8) + 6].GetComponent<TextMeshProUGUI>().text = laz[0] + "." + laz[1] + " °C";
                    }
                    else
                    {
                        gridTexts[(count * 8) + 6].GetComponent<TextMeshProUGUI>().text = "0" + "." + dataList[i].deger.ToString() + " °C";
                    }

                }
                else if (dataList[i].ad.ToLower() == "basınc")
                {
                    grd1 = dataList[i].deger.ToString();

                    gridTexts[(count * 8) + 7].GetComponent<TextMeshProUGUI>().text = grd1 + " hPa";
                }
                else if (dataList[i].ad.ToLower() == "nem")
                {

                    if (lenn >= 3)
                    {
                        for (int x = lenn - 1; x < lenn; x++)
                        {
                            grd1 += laz[x];
                        }
                        for (int x = 0; x < lenn - 1; x++)
                        {
                            grd2 += laz[x];
                        }
                        gridTexts[(count * 8) + 8].GetComponent<TextMeshProUGUI>().text = grd2 + "." + grd1 + " %";
                    }
                    else if (lenn == 2)
                    {
                        gridTexts[(count * 8) + 8].GetComponent<TextMeshProUGUI>().text = laz[0] + "." + laz[1] + " %";
                    }
                    else
                    {
                        gridTexts[(count * 8) + 8].GetComponent<TextMeshProUGUI>().text = "0" + "." + dataList[i].deger.ToString() + " %";
                    }
                }
                i++;
            }
            nowid = dataList[i].id;
            i--;
            count++;
        }
    }
    private void FillTables()
    {
        for (int i = 0; i < gridTexts.Count; i++)
        {
            if (stationTexts[i]!= null) { 
                stationTexts[i].GetComponent<TextMeshPro>().text = gridTexts[i].GetComponent<TextMeshProUGUI>().text;
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
                    UnityEngine.Debug.LogError(pages[page] + ": Error: " + webRequest.error);

                    break;
                case UnityWebRequest.Result.ProtocolError:
                    UnityEngine.Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string raw = webRequest.downloadHandler.text;
                    string[] qwe = raw.Split(",");
                    dataList.Clear();
                    for (int i = 0; i < qwe.Length - 1; i++)
                    {
                        string[] ert = qwe[i].Split("/");
                        data wer = new data();
                        wer.id = Convert.ToInt32(ert[0]);
                        wer.ad = ert[1];
                        wer.deger = Convert.ToInt32(ert[2]);
                        dataList.Add(wer);
                        dataList.Sort();
                    }
                    break;
            }
        }
    }
}
