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

    [Header("Internet")]
    public Image Internetimg;
    public Sprite news1;
    public Sprite ipconfig;
    [SerializeField] MonologueTrigger monologueTrigger;
    [SerializeField] GameObject JiHyeMessage;
    [SerializeField] GameObject CrimePic;
    [SerializeField] GameObject VictimPic;
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
        else if(option.value.Equals(0))
        {
            MapImage.sprite = hereMap;
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
            NoteBut2.text = "���� �ʰ� �ʵ� ����. �� �ǵ��� ����°���?";
        }
    }
    public void Day4CheckMessage()
    {
        monologueTrigger.TriggerMonologue("Day4CheckMes");
        GameStats.Instance.Stage4CheckMessage = true;
    }
}
