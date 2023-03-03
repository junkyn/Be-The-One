using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    Color transparent = new Color(1, 1, 1, 0);

    [SerializeField] RectTransform screenCurrent;
    Vector3 screenPos;
    WaitForSeconds appDelay;
    [SerializeField] Image fadeImage;

    [Header("Story")]
    [SerializeField] StoryManager storyManager;

    [Header("Lock Screen")]
    [SerializeField] GameObject clickBlock;
    [SerializeField] RectTransform screenLocked, screenMain;

    [SerializeField] Image iconLock;
    [SerializeField] Sprite spriteUnlock, spriteLock;
    [SerializeField] TextMeshProUGUI touchToUnlock;

    [SerializeField] TextMeshProUGUI dateText, dayText;

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timerText;
    public float time = 0;
    int currentMinute = 0, currentHour = 2;
    new WaitForSeconds timer;

    /*
    [Header("Game Stats")]
    int _date;
    int month = 7;
    string[] daylist = { "��", "ȭ", "��", "��", "��", "��", "��" };
    string day;
    */

    [Header("Monologue")]
    [SerializeField] MonologueManager monologueManager;
    [SerializeField] MonologueTrigger monologueTrigger;

    [Header("Gallery")]
    [SerializeField] Image galleryZoomed;
    Vector3 galleryZoomedPos;
    bool galleryBinActivated;
    [SerializeField] RectTransform galleryBinButton;
    [SerializeField] TextMeshProUGUI galleryBinButtonText;
    [SerializeField] GameObject galleryDeleteButton;

    GameObject photoCurrent;
    [SerializeField] GameObject photoPrefab;
    [SerializeField] Transform galleryContent, binContent;

    [Header("Call")]
    [SerializeField] TextMeshProUGUI callDialText;

    [Header("Message")]
    [SerializeField] ChatTrigger chatTrigger;
    [SerializeField] Chat chatCurrent;
    [SerializeField] GameObject chatPrefab;
    [SerializeField] TextMeshProUGUI chatName;
    [SerializeField] Transform chatContent;
    [SerializeField] ScrollRect chatScrollRect;

    [SerializeField] RectTransform chatRectTransform;
    bool replying = false;
    [SerializeField] Button replyButton;
    [SerializeField] TextMeshProUGUI reply1, reply2, reply3;

    [Header("Setting")]
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource fx;
    [SerializeField] Slider bgmSlider, fxSlider;
    [SerializeField] Toggle slowToggle, fastToggle;

    void Start()
    {
        touchToUnlock.DOColor(transparent, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        //DateSet();

        timer = new WaitForSeconds(1);

        monologueTrigger.TriggerMonologue("Intro");

        screenPos = screenMain.position;
        appDelay = new WaitForSeconds(.55f);

        galleryZoomedPos = galleryZoomed.transform.position;
    }

    /*��¥�� ������Ʈ�մϴ�.
    void DateSet()
    {
        _date = GameStats.Date;
        day = daylist[_date % 7];
        if (_date > 31)
        {
            month++;
            _date -= 31;
        }
        dateText.text = month + "�� " + _date + "��";
        dayText.text = day + "����";
    }
    */

    //����� ������ �������� �ð��� ������Ʈ�մϴ�.
    IEnumerator TimeCheck()
    {
        yield return timer;
        time++;

        if (time >= 60)
        {
            if (currentMinute < 60)
            {
                currentMinute++;
                if (currentMinute >= 60)
                {
                    currentHour++;
                    currentMinute = 0;
                }
                time = 0;
            }
            string minute = string.Empty;
            if (currentMinute < 10)
                minute = "0" + currentMinute.ToString();
            else
                minute = currentMinute.ToString();

            timerText.text = currentHour.ToString() + ":" + minute;
        }

        StartCoroutine(TimeCheck());
    }

    //����� �����մϴ�.
    public void Unlock()
    {
        clickBlock.SetActive(true);
        StartCoroutine(UnlockCoroutine());
    }

    IEnumerator UnlockCoroutine()
    {
        iconLock.sprite = spriteUnlock;

        yield return new WaitForSeconds(.3f);

        screenLocked.DOPivotY(-.5f, 0);
        screenLocked.DOMove(screenPos, .5f);
        screenMain.gameObject.SetActive(true);

        if(GameStats.Instance.Stage.Equals(0))
            monologueTrigger.TriggerMonologue("Unlocked");

        yield return appDelay;
        screenLocked.gameObject.SetActive(false);
        screenLocked.DOPivotY(.5f, 0);
        screenLocked.DOMove(screenPos, 0);

        clickBlock.SetActive(false);
        StartCoroutine(TimeCheck());
    }
    
    //����� �����մϴ�.
    void Lock()
    {
        iconLock.sprite = spriteLock;
        screenLocked.gameObject.SetActive(true);

        screenCurrent.DOPivotX(.5f, 0);
        screenCurrent.DOMove(screenPos, 0);

        screenCurrent.DOPivotY(1.5f, 0);
        screenCurrent.DOMove(screenPos, 0);
        screenCurrent.gameObject.SetActive(false);
        screenCurrent.DOMove(screenPos, 0);

        screenMain.gameObject.SetActive(false);
    }

    //���� �����մϴ�.
    public void AppStart(RectTransform app)
    {
        clickBlock.SetActive(true);
        screenCurrent = app;
        StartCoroutine(AppStartCoroutine());
    }

    IEnumerator AppStartCoroutine()
    {
        screenCurrent.DOPivotY(1.5f, 0);
        screenCurrent.DOMove(screenPos, 0);
        screenCurrent.gameObject.SetActive(true);
        screenCurrent.DOPivotY(.5f, 0);
        screenCurrent.DOMove(screenPos, .5f);
        yield return appDelay;
        clickBlock.SetActive(false);
    }

    //���� �����մϴ�.
    public void AppShutdown()
    {
        clickBlock.SetActive(true);
        StartCoroutine(AppShutdownCoroutine());
    }

    IEnumerator AppShutdownCoroutine()
    {
        screenCurrent.DOPivotY(1.5f, 0);
        screenCurrent.DOMove(screenPos, .5f);
        yield return appDelay;
        screenCurrent.gameObject.SetActive(false);
        screenCurrent.DOMove(screenPos, 0);
        clickBlock.SetActive(false);
    }

    //���������� ������ Ȯ���մϴ�.
    public void GalleryZoomIn(Image image)
    {
        galleryZoomed.sprite = image.sprite;
        galleryZoomed.gameObject.SetActive(true);

        photoCurrent = image.gameObject;
        if (image.name.Equals("crimeplace1") && !GameStats.Instance.Stage2CheckGallery)
        {
            storyManager.Stage2CheckGallery();
        }
    }

    //���������� �������� Ȱ��ȭ�մϴ�.
    //Ȱ��ȭ�Ǿ����� ���, ���������� �̵��մϴ�.
    public void GalleryBinActivate()
    {
        if (!galleryBinActivated)
        {
            galleryBinActivated = true;

            galleryBinButton.DOSizeDelta(new Vector2(65, 35), 0);
            galleryBinButtonText.text = "������";
            galleryDeleteButton.SetActive(true);
        }
        else
        {
            clickBlock.SetActive(true);
            StartCoroutine(GalleryBinActivateCoroutine());
        }
    }

    IEnumerator GalleryBinActivateCoroutine()
    {
        screenCurrent.DOPivotX(1.5f, 0);
        screenCurrent.DOMove(screenPos, .5f);

        yield return appDelay;
        galleryZoomed.transform.DOMove(galleryZoomedPos, 0);
        clickBlock.SetActive(false);
    }

    //���������� �������� ����ϴ�.
    public void GalleryBinDeactivate()
    {
        clickBlock.SetActive(true);
        StartCoroutine(GalleryBinDeactivateCoroutine());
    }

    IEnumerator GalleryBinDeactivateCoroutine()
    {
        screenCurrent.DOPivotX(.5f, 0);
        screenCurrent.DOMove(screenPos, .5f);

        yield return appDelay;
        galleryZoomed.transform.DOMove(galleryZoomedPos, 0);
        clickBlock.SetActive(false);
    }

    //���������� ������ �����մϴ�.
    public void GalleryDelete()
    {
        GameObject deletedPhoto = Instantiate(photoPrefab, binContent);
        deletedPhoto.GetComponent<Image>().sprite = photoCurrent.GetComponent<Image>().sprite;
        deletedPhoto.transform.DOScale(Vector3.one, 0);

        deletedPhoto.GetComponent<Button>().onClick.AddListener(() => GalleryZoomIn(deletedPhoto.GetComponent<Image>()));

        Destroy(photoCurrent.gameObject);
    }

    //��ȭ ���� ������ ��� �Էµ� ������ ��� ����ϴ�.
    public void CallDialReset()
    {
        callDialText.text = "";
    }

    //��ȭ �ۿ��� ���̾� �Է��� �޽��ϴ�.
    public void CallDialInput(string dial)
    {
        if (callDialText.text.Length < 20)
            callDialText.text += dial;
    }

    //��ȭ �ۿ��� �Էµ� ���̾��� ����ϴ�.
    public void CallDialDelete()
    {
        if (callDialText.text != "")
            callDialText.text = callDialText.text.Remove(callDialText.text.Length - 1);
    }

    //�޽��� �ۿ��� ä��â���� �̵��մϴ�.
    public void MessageChatActivate(string name)
    {
        clickBlock.SetActive(true);
        chatTrigger.TriggerChat(name);
    }

    //ä�� ������ �ҷ��ɴϴ�.
    public IEnumerator MessageChatUpdate(Chat chat)
    {
        chatCurrent = chat;

        for (int i = 0; i < chatContent.childCount; i++)
            Destroy(chatContent.GetChild(i).gameObject);

        chatName.text = "< " + chat.name;
        for (int i = 0; i < chat.sentences.Count; i++)
        {
            GameObject newChat = Instantiate(chatPrefab, chatContent);
            newChat.GetComponent<TextMeshProUGUI>().text= chat.sentences[i];
            newChat.transform.DOScale(Vector3.one, 0);

            if ((i % 2).Equals(0))
                newChat.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
            else
                newChat.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        }

        if (!chat.replyable)
            replyButton.interactable = false;
        else
            replyButton.interactable = true;

        Canvas.ForceUpdateCanvases();
        chatScrollRect.verticalNormalizedPosition = 0;

        screenCurrent.DOPivotX(1.5f, 0);
        screenCurrent.DOMove(screenPos, .5f);

        yield return appDelay;
        clickBlock.SetActive(false);
    }

    public void MessageChatReply()
    {
        if (chatName.text.Equals("< �Ǳ� ���ź���"))
            monologueTrigger.TriggerMonologue("Asylum");
        else
        {
            if (!replying)
            {
                replying = true;

                chatRectTransform.DOSizeDelta(new Vector2(0, -120), .5f).SetRelative();
                replyButton.transform.DOMoveY(340, .5f).SetRelative();
                replyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "���";

                ReplyCase("< ������");
            }
            else
            {
                replying = false;

                chatRectTransform.DOSizeDelta(new Vector2(0, 120), .5f).SetRelative();
                replyButton.transform.DOMoveY(-340, .5f).SetRelative();
                replyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "�޽��� ������";
            }
        }
    }

    void ReplyCase(string name)
    { 
        switch(name)
        {
            case "< ������":
                if (GameStats.Instance.Stage.Equals(2))
                    ReplyGenerate("�ƴ�?", "(�������� �ʴ´�.)", "");
                break;
        }
    }

    void ReplyGenerate(string reply1Text, string reply2Text, string reply3Text)
    {
        reply2.transform.parent.gameObject.SetActive(true);
        reply3.transform.parent.gameObject.SetActive(true);

        reply1.text = reply1Text;
        reply2.text = reply2Text;
        reply3.text = reply3Text;

        if (reply3.text.Equals(""))
            reply3.transform.parent.gameObject.SetActive(false);
        if (reply2.text.Equals(""))
            reply2.transform.parent.gameObject.SetActive(false);
    }

    //������ �����ϴ�.
    public void Reply(TextMeshProUGUI text)
    {
        string reply = text.text;
        if (GameStats.Instance.Stage.Equals(2))
        {
            if (reply.Equals("(�������� �ʴ´�.)"))
            {
                chatCurrent.replyable = false;
                GameStats.Instance.Stage=3;
            }
            else
            {
                chatCurrent.replyable = false;
                chatCurrent.sentences.Add("-----------------------\n\n" + reply + "\n\n-----------------------");
                StartCoroutine(MessageChatUpdate(chatCurrent));
                GameStats.Instance.Stage=4;
            }
        }


        MessageChatReply();
        if (GameStats.Instance.Stage.Equals(3)||GameStats.Instance.Stage.Equals(4))
            StartCoroutine(NextDay());
    }

    //�޽��� �ۿ��� ä��â�� ����ϴ�.
    public void MessageChatDeactivate()
    {
        clickBlock.SetActive(true);
        StartCoroutine(MessageChatDeactivateCoroutine());

        for (int i = 0; i < chatContent.childCount; i++)
            Destroy(chatContent.GetChild(0).gameObject);
    }

    IEnumerator MessageChatDeactivateCoroutine()
    {
        screenCurrent.DOPivotX(.5f, 0);
        screenCurrent.DOMove(screenPos, .5f);

        yield return appDelay;
        clickBlock.SetActive(false);
    }

    //������ �����մϴ�.
    public void Settings()
    {
        bgm.volume = bgmSlider.value;
        fx.volume = fxSlider.value;

        if (slowToggle.isOn)
        {
            monologueManager.typeSpeed = .13f;
            monologueManager.TypeSpeedUpdate();
        }
        else if (fastToggle.isOn)
        {
            monologueManager.typeSpeed = .01f;
            monologueManager.TypeSpeedUpdate();
        }
    }

    IEnumerator NextDay()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(new Color(0, 0, 0, 1), 4f);
        yield return new WaitForSeconds(4.5f);

        storyManager.day++;
        dayText.text = "8�� " + storyManager.day.ToString() + "��";
        switch(storyManager.day % 7)
        {
            case 1:
                dateText.text = "ȭ����";
                break;
            case 2:
                dateText.text = "������";
                break;
            case 3:
                dateText.text = "�����";
                break;
            case 4:
                dateText.text = "�ݿ���";
                break;
            case 5:
                dateText.text = "�����";
                break;
            case 6:
                dateText.text = "�Ͽ���";
                break;
            case 0:
                dateText.text = "������";
                break;
        }
        Lock();

        screenLocked.gameObject.SetActive(true);

        fadeImage.DOColor(new Color(0, 0, 0, 0), .5f);
        yield return new WaitForSeconds(.5f);
        fadeImage.gameObject.SetActive(false);
        OpenDaySet();
    }
    
    public void OpenDaySet()
    {
        switch (GameStats.Instance.Stage)
        {
            case 3:
                monologueTrigger.TriggerMonologue("OpenDay2");
                chatTrigger.Stage3JiHye();
                storyManager.Day2Update();
       
                break;
            case 4:
                monologueTrigger.TriggerMonologue("OpenDay2");
                storyManager.Day2DeleteJiHye();
                storyManager.Day2Update();
                break;
        }
    }
    public void CheckEnding()
    {
        if (GameStats.Instance.CheckClear(GameStats.Instance.Stage))
        {
            StartCoroutine(NextDay());
        }
    }
    private void Update()
    {
        CheckEnding();
    }
}
