using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public int day = 0;
    public TMP_Dropdown dropdown;

    [Header("Map")]
    public Image MapImage;
    public Sprite hereMap;
    public Sprite hongdaeMap;
    public Sprite ItaewonMap;
    public Sprite NonhyeonMap;
    [Header("Internet")]
    public Image Internetimg;
    public Sprite news1;
    public Sprite ipconfig;
    public Sprite news2;

    [Header("Ending")]
    [SerializeField] GameObject EndingScreen;
    public Image EndingImg;
    public Sprite Ending1;
    public Sprite Ending2;
    public Sprite Ending3;
    public Sprite Ending4;
    public Sprite Ending5;
    public Sprite Ending6;
    public Sprite Ending7;
    public Sprite Ending8;
    [SerializeField] MonologueTrigger monologueTrigger;
    [SerializeField] GameObject JiHyeMessage;
    [Header("Photos")]
    [SerializeField] GameObject CrimePic;
    [SerializeField] GameObject VictimPic;
    [SerializeField] GameObject Screenshot;
    

    [SerializeField] GameObject HyejinMessage;
    [Header("Note")]
    [SerializeField] TextMeshProUGUI NoteText;
    [SerializeField] TextMeshProUGUI NoteBut1;
    [SerializeField] TextMeshProUGUI NoteBut2;
    [SerializeField] TextMeshProUGUI NoteBut3;

    
    public void FirstOpenID()
    {
        if (GameStats.Instance.Stage.Equals(0))
        {
            monologueTrigger.TriggerMonologue("CheckName");
        }
    }
    public void FirstOutID()
    {
        if(GameStats.Instance.Stage.Equals(0))
          StartCoroutine(FirstDayEvent());
    }
    IEnumerator FirstDayEvent()
    {
        yield return new WaitForSeconds(3f);
        {
            monologueTrigger.TriggerMonologue("GetMessage");
            JiHyeMessage.SetActive(true);
        }

        GameStats.Instance.Stage = 1;
    }

    public void OpenFirstJiHye()
    {
        monologueTrigger.TriggerMonologue("FirstDayJiHye");

        GameStats.Instance.Stage = 2;
    }
    public void Stage3OpenJiHye()
    {
        monologueTrigger.TriggerMonologue("2_XAns_CheckMe");
        GameStats.Instance.Stage2CheckMessage= true;
    }
    public void Day2DeleteJiHye()
    {
        JiHyeMessage.SetActive(false);
    }

    public void Stage4CheckMessage()
    {
        monologueTrigger.TriggerMonologue("2_Ans_CheckMe");
        GameStats.Instance.Stage2CheckMessage= true;
    }
    public void Stage2CheckGallery()
    {
        monologueTrigger.TriggerMonologue("CheckCrimePi");
        GameStats.Instance.Stage2CheckGallery= true;
    }
    public void Day2Update()
    {
        TMP_Dropdown.OptionData HongDae = new TMP_Dropdown.OptionData();
        HongDae.text = "ȫ���Ա���";
        HongDae.image = null;
        dropdown.options.Add(HongDae);
        CrimePic.SetActive(true);
    }
    public void Day3Update()
    {
        if (GameStats.Instance.Stage == 5)
        {
            Internetimg.sprite = news1;
            JiHyeMessage.SetActive(false);
            VictimPic.SetActive(true);
        }
        else if(GameStats.Instance.Stage == 6)
        {
            Internetimg.sprite = ipconfig;
        }
    }
    public void MapChange(TMP_Dropdown option)
    {
        if (option.value.Equals(1))
        {
            if (!GameStats.Instance.Stage2CheckMap)
            {
                monologueTrigger.TriggerMonologue("CheckMapDay2");
                GameStats.Instance.Stage2CheckMap = true;
            }
            MapImage.sprite = hongdaeMap;
        }
        else if (option.value.Equals(0))
        {
            MapImage.sprite = hereMap;
        }
        else if (option.value.Equals(2))
        {
            if (GameStats.Instance.Stage == 25)
                MapImage.sprite = ItaewonMap;
            else if(GameStats.Instance.Stage == 26 || GameStats.Instance.Stage == 27)
            {
                MapImage.sprite = NonhyeonMap;
            }
        }

    }
    public void Stage3CheckInternet()
    {
        if(!GameStats.Instance.Stage3CheckInternet)
        {
            if(GameStats.Instance.Stage == 5)
            {
                monologueTrigger.TriggerMonologue("CheckNews");
                GameStats.Instance.Stage3CheckInternet = true;
            }
            else if(GameStats.Instance.Stage == 6)
            {
                monologueTrigger.TriggerMonologue("CheckIP");
                GameStats.Instance.Stage3CheckInternet = true;
            }

        }
    }
    public void Stage3CheckGallery()
    {
        monologueTrigger.TriggerMonologue("CheckVictim");
        GameStats.Instance.Stage3CheckGallery = true;
    }
    
    public void Day4Update()
    {
        CrimePic.SetActive(false);
        if(VictimPic.activeSelf)
            VictimPic.SetActive(false);
        if (GameStats.Instance.Stage.Equals(7))
        {
            NoteText.text = "�ȳ�?";
            NoteBut1.text = "���� �ʰ� ���ΰž�?";
            NoteBut2.text = "�� �����ָ� �ɱ�?";
            NoteBut3.text = "(������ ����)";
        }
        else if (GameStats.Instance.Stage.Equals(8))
        {
            NoteText.text = "������ � ���� �ִ��� �޴����� �ǵ�����.";
            NoteBut1.text = "�˾Ҿ�";
            NoteBut2.text = "��ü ��? �� ���ε�..";
        }
    }
    public void Day4CheckMessage()
    {
        monologueTrigger.TriggerMonologue("Day4CheckMes");
        GameStats.Instance.Stage4CheckMessage = true;
    }
    public void Day5Update()
    {
        if (GameStats.Instance.Stage.Equals(9))
        {
            NoteText.text = "���� �� �ñ���? �¾� ���� �׿��� �ʰ� �׿���. ���� ������ �ʵ� ������ �Ŷ�. �� �� �����ٷ�?";
            NoteBut1.text = "��ü �� �̷� ���� �ϴ°ž�?";
            NoteBut2.text = "�� �ϸ� �ɱ�?";
            NoteBut3.text = "";
        }
        else if (GameStats.Instance.Stage.Equals(10))
        {
            NoteBut1.text = "";
            NoteBut2.text = "";
            NoteBut3.text = "";
            StartCoroutine(HyejinEvent());
        }
        else if (GameStats.Instance.Stage.Equals(11))
        {
            NoteText.text = "�����ϸ� ���� �ǵ�����.";
            NoteBut1.text = "";
            NoteBut2.text = "";
            NoteBut3.text = "";
        }
        else if (GameStats.Instance.Stage.Equals(12))
        {
            NoteBut1.text = "";
            NoteBut2.text = "";
            NoteBut3.text = "";
        }
        else if (GameStats.Instance.Stage.Equals(13))
        {
            NoteText.text = "��.. ���� ���� �谨�� �ʵ� �������� �� ���µ�.. ��� �̷��� �� �� ���ݾ�? ������ �� �� ���������� ���ϴ� �� ������ �׳� ���� �϶�� ��� ���� ��.";
            NoteBut1.text = "";
            NoteBut2.text = "";
            NoteBut3.text = "";
        }

    }
    IEnumerator HyejinEvent()
    {
        yield return new WaitForSeconds(3f);
        {
            monologueTrigger.TriggerMonologue("GetMessage");
            HyejinMessage.SetActive(true);

        }

    }
    public void HyeJinScreenShot()
    {
        monologueTrigger.TriggerMonologue("AfterScreenShot");
        GameStats.Instance.Stage5CheckMessage = true;
        Screenshot.SetActive(true);
    }
    public void ToDeleteMonol()
    {
        monologueTrigger.TriggerMonologue("ToDeletePhoto");
    }
    public void Day6Update()
    {
        TMP_Dropdown.OptionData nonhyeon = new TMP_Dropdown.OptionData();
        nonhyeon.text = "�ų�����";
        nonhyeon.image = null;
        switch (GameStats.Instance.Stage)
        {
            case 17:
                NoteText.text = "������ �� ã�ƿԾ�. �����㿡 ��ü �� �Ѱž�? ���� ���̻� �����ְھ�.";
                Internetimg.sprite = news2;
                GameStats.Instance.Stage = 23;
                break;
            case 18:
                Internetimg.sprite = news2;
                GameStats.Instance.Stage = 24;
                break;
            case 19:
                monologueTrigger.TriggerMonologue("fellsome");
                NoteText.text = "��ģ�ž�? ������ �ִ°� �׷��� �����? ó���ϴ��� �������.";
                Internetimg.sprite = news2;
                GameStats.Instance.Stage = 25;

                TMP_Dropdown.OptionData Itaewon = new TMP_Dropdown.OptionData();
                Itaewon.text = "���¿���";
                Itaewon.image = null;
                dropdown.options.Add(Itaewon);
                break;
            case 20:
                Internetimg.sprite = news2;
                GameStats.Instance.Stage = 26;

                dropdown.options.Add(nonhyeon);
                break;
            case 21:
                monologueTrigger.TriggerMonologue("call112");

                Internetimg.sprite = news2;
                GameStats.Instance.Stage = 27;

                dropdown.options.Add(nonhyeon);
                break;
            case 22:
                GameStats.Instance.Stage = 28;

                Internetimg.sprite = news2;
                break;
        }
    }
    public void Ending29()
    {
        EndingScreen.SetActive(true);
        EndingImg.sprite = Ending2;
    }
    public void Ending30()
    {
        EndingScreen.SetActive(true);
        EndingImg.sprite = Ending3;

    }
    public void Ending31()
    {
        EndingScreen.SetActive(true);
        
        EndingImg.sprite = Ending4;

    }
    public void Ending32()
    {
        EndingScreen.SetActive(true);
        EndingImg.sprite = Ending5;

    }
    public void Ending33()
    {
        EndingScreen.SetActive(true);
        EndingImg.sprite = Ending6;


    }
    public void Ending34()
    {
        EndingScreen.SetActive(true);
        EndingImg.sprite = Ending6;

    }
    public void Ending35()
    {
        EndingScreen.SetActive(true);
        EndingImg.sprite = Ending7;

    }
    public void Ending36()
    {
        EndingScreen.SetActive(true);
        EndingImg.sprite = Ending8;

    }
    public void Ending37()
    {
        EndingScreen.SetActive(true);
        EndingImg.sprite = Ending1;
    }
}
