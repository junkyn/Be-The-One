using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.IO;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    Color transparent = new Color(1, 1, 1, 0);

    [SerializeField] GameObject screenContinue, screenWarning;

    [SerializeField] RectTransform screenCurrent;
    Vector3 screenPos;
    WaitForSeconds appDelay;
    [SerializeField] Image fadeImage;


    [System.Serializable]
    public class SaveData
    {
        public int stage;
        public int day;
    }
    string path;
    public int stageSaved;
    public int daySaved;

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

    [Header("Monologue")]
    [SerializeField] MonologueManager monologueManager;
    [SerializeField] MonologueTrigger monologueTrigger;
    public GameObject Monologue;

    [Header("Gallery")]
    [SerializeField] Image galleryZoomed;
    Vector3 galleryZoomedPos;
    bool galleryBinActivated;
    [SerializeField] RectTransform galleryBinButton;
    [SerializeField] TextMeshProUGUI galleryBinButtonText;
    [SerializeField] GameObject galleryDeleteButton;
    [SerializeField] GameObject GalleryScreen;
    GameObject photoCurrent;
    [SerializeField] GameObject photoPrefab;
    [SerializeField] Transform galleryContent, binContent;

    [Header("Call")]
    [SerializeField] TextMeshProUGUI callDialText;
    [SerializeField] TextMeshProUGUI callIncoming, calling;

    public List<string> numbersEnable;
    [SerializeField] GameObject callScreen;
    [SerializeField] GameObject callPrefab;
    [SerializeField] Transform callContent;
    [SerializeField] ScrollRect callScrollRect;
    [SerializeField] TextMeshProUGUI callReply1, callReply2, callReply3;
    [SerializeField] GameObject callEnd;

    [Header("Message")]
    [SerializeField] ChatTrigger chatTrigger;
    [SerializeField] Chat chatCurrent;
    [SerializeField] GameObject chatPrefab;
    [SerializeField] TextMeshProUGUI chatName;
    [SerializeField] Transform chatContent;
    [SerializeField] ScrollRect chatScrollRect;
    [SerializeField] GameObject MessageScreen;
    [SerializeField] RectTransform chatRectTransform;
    bool replying = false;
    [SerializeField] Button replyButton;
    [SerializeField] TextMeshProUGUI reply1, reply2, reply3;

    [Header("Note")]
    [SerializeField] TextMeshProUGUI noteText;
    [SerializeField] Transform noteButtons;
    [SerializeField] TextMeshProUGUI note1, note2, note3;
    [SerializeField] GameObject noteButton1;
    [SerializeField] GameObject noteButton2;
    [SerializeField] GameObject noteButton3;
    [SerializeField] GameObject NoteScreen;

    [Header("Setting")]
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource fx;
    [SerializeField] Slider bgmSlider, fxSlider;
    [SerializeField] Toggle slowToggle, fastToggle;

    public GameObject MapScreen;
    public GameObject InternetScreen;
    private void Start()
    {
        path = Path.Combine(Application.dataPath, "database.json");
        SaveData saveData = new SaveData();

        if (!File.Exists(path))
        {
            stageSaved = 0;
            daySaved = 0;
            SaveGame();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                stageSaved = saveData.stage;
                daySaved = saveData.day;
            }
        }

        touchToUnlock.DOColor(transparent, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        screenPos = screenMain.position;
        appDelay = new WaitForSeconds(.55f);

        galleryZoomedPos = galleryZoomed.transform.position;
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.stage = GameStats.Instance.Stage;
        saveData.day = storyManager.day;

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path, json);
    }

    public void LoadGame(bool load)
    {
        if (load)
        {
            GameStats.Instance.Stage = stageSaved;
            storyManager.day = daySaved - 1;

            StartCoroutine(GameStartCoroutine());
        }
        else
        {
            if (stageSaved != 0 || daySaved != 0)
                screenWarning.SetActive(true);
            else
                NewGame();
        }
    }

    public void NewGame()
    {
        StartCoroutine(GameStartCoroutine());
    }

    IEnumerator GameStartCoroutine()
    {
        StartCoroutine(NextDay());
        yield return new WaitForSeconds(3.5f);

        screenContinue.SetActive(false);
        GameStart();
    }

    void GameStart()
    {
        timer = new WaitForSeconds(1);

        if (GameStats.Instance.Stage.Equals(0))
            monologueTrigger.TriggerMonologue("Intro");
    }

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
        if (image.sprite.name.Equals("crimeplace1") && !GameStats.Instance.Stage2CheckGallery)
        {
            storyManager.Stage2CheckGallery();
        }
        else if(image.sprite.name.Equals("victim")&& !GameStats.Instance.Stage3CheckGallery)
        {
            storyManager.Stage3CheckGallery();
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

    public void CallIncome(string number)
    {
        callScreen.SetActive(true);
        callIncoming.transform.parent.gameObject.SetActive(true);
        callIncoming.text = number;
    }

    public void CallTake(TextMeshProUGUI number)
    {
        calling.text = number.text;
        CallStart(number.text);
    }

    public void Call(TextMeshProUGUI number)
    {
        if (numbersEnable.Contains(number.text))
        {
            calling.text = number.text;
            CallStart(number.text);
        }
    }

    public void CallStart(string number)
    {
        switch (number)
        {
            case "01012345678":
                if (GameStats.Instance.Stage.Equals(0))
                    StartCoroutine(CallCoroutine("��ȭ �����Դϴ�.", "����1", "����2", "����3"));
                if (GameStats.Instance.Stage.Equals(1))
                    StartCoroutine(CallCoroutine("��ȭ ����2�Դϴ�.", "����2-1", "����2-2", "����2-3"));
                if (GameStats.Instance.Stage.Equals(2))
                    StartCoroutine(CallCoroutine("��ȭ�� �����ϴ�.", "����1", "����2", "��ȭ�� ���´�."));
                if (GameStats.Instance.Stage.Equals(3))
                    StartCoroutine(CallCoroutine("��ȭ�� ����ϴ�.", "����1", "����2", "����3", true));
                break;
        }
    }

    public IEnumerator CallCoroutine(string sentence, string reply1, string reply2, string reply3, bool hangUp = false)
    {
        yield return appDelay;

        GameObject call = Instantiate(callPrefab, callContent);
        call.GetComponent<TextMeshProUGUI>().text = sentence;

        if (!hangUp)
            CallReplyUpdate(reply1, reply2, reply3);
        else
            CallHangUp();
    }

    void CallReplyUpdate(string reply1, string reply2, string reply3)
    {
        callReply1.transform.parent.GetComponent<Button>().interactable = true;
        callReply2.transform.parent.GetComponent<Button>().interactable = true;
        callReply3.transform.parent.GetComponent<Button>().interactable = true;

        callReply1.text = reply1;
        callReply2.text = reply2;
        callReply3.text = reply3;
    }

    public void CallReply(TextMeshProUGUI reply)
    {
        if (reply.text != "��ȭ�� ���´�.")
        {
            callReply1.transform.parent.GetComponent<Button>().interactable = false;
            callReply2.transform.parent.GetComponent<Button>().interactable = false;
            callReply3.transform.parent.GetComponent<Button>().interactable = false;

            GameObject callReply = Instantiate(callPrefab, callContent);
            callReply.GetComponent<TextMeshProUGUI>().text = reply.text;
            callReply.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;

            Canvas.ForceUpdateCanvases();
            callScrollRect.verticalNormalizedPosition = 0;

            GameStats.Instance.Stage++;
            CallStart(calling.text);
        }
        else
        {
            CallHangUp();

            GameStats.Instance.Stage++;
        }
    }

    public void CallHangUp()
    {
        callReply1.transform.parent.GetComponent<Button>().interactable = false;
        callReply2.transform.parent.GetComponent<Button>().interactable = false;
        callReply3.transform.parent.GetComponent<Button>().interactable = false;

        monologueTrigger.TriggerMonologue("HangUp");
        callEnd.SetActive(true);
    }

    public void CallReset()
    {
        foreach (Transform chats in callContent)
            Destroy(chats.gameObject);

        callScreen.SetActive(false);
        calling.transform.parent.gameObject.SetActive(false);
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
            if (chat.name.Equals("������"))
            {
                if ((i % 2).Equals(0))
                    newChat.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
                else
                    newChat.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
            }
            else
            {
                newChat.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
            }

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

    //�޸� �Է��մϴ�.
    public void NoteWrite(TextMeshProUGUI note)
    {
        
        string noteToWrite = note.text;

        if(!note.text.Equals("(������ ����)"))
        {
            noteText.text = noteToWrite;
        }
        if(note.text.Equals("���� �ʰ� ���ΰž�?"))
        {
            GameStats.Instance.Route = 1;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if(note.text.Equals("(������ ����)"))
        {
            GameStats.Instance.Route = 2;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if (note.text.Equals("�� �����ָ� �ɱ�?"))
        {
            GameStats.Instance.Route = 3;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if (note.text.Equals("�˾Ҿ�"))
        {
            GameStats.Instance.Route = 4;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if (note.text.Equals("��ü ��? �� ���ε�.."))
        {
            GameStats.Instance.Route = 5;
            GameStats.Instance.Stage4CheckMemo = true;
        }
        else if(note.text.Equals("��ü �� �̷� ���� �ϴ°ž�?"))
        {
            GameStats.Instance.Route = 1;
        }
        else if(note.text.Equals("�� �ϸ� �ɱ�?"))
        {
            GameStats.Instance.Route = 2;
        }
        noteButtons.DOMoveY(-340, .5f).SetRelative();


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
        fadeImage.DOColor(new Color(0, 0, 0, 1), 3f);
        yield return new WaitForSeconds(3.5f);

        SaveGame();

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


        //��Ʈ ������ �����մϴ�.
        noteButton1.SetActive(true);
        noteButton2.SetActive(true);
        noteButton3.SetActive(true);
        //noteButtons.DOMoveY(340, .5f).SetRelative();
        note1.text = "";
        note2.text = "";
        note3.text = "";
        noteButtons.DOMoveY(340, .5f).SetRelative();

        fadeImage.DOColor(new Color(0, 0, 0, 0), .5f);
        yield return new WaitForSeconds(.5f);
        fadeImage.gameObject.SetActive(false);
        OpenDaySet();
        if (note1.text.Equals(""))
            noteButton1.SetActive(false);
        if (note2.text.Equals(""))
            noteButton2.SetActive(false);
        if (note3.text.Equals(""))
            noteButton3.SetActive(false);

    }

    public void OpenDaySet()
    {
        Debug.Log(noteButtons.transform.position.y);
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
            case 5:
                monologueTrigger.TriggerMonologue("OpenDay3");
                storyManager.Day3Update();
                break;
            case 6:
                monologueTrigger.TriggerMonologue("OpenDay3_2");
                storyManager.Day3Update();
                break;
            case 7:
                monologueTrigger.TriggerMonologue("OpenDay4");
                chatTrigger.Stage4Ashylum();
                storyManager.Day4Update();
                break;
            case 8:
                monologueTrigger.TriggerMonologue("OpenDay4");
                chatTrigger.Stage4Ashylum();
                storyManager.Day4Update();
                break;
            case 9:
                monologueTrigger.TriggerMonologue("OpenDay5");
                storyManager.Day5Update();
                break;
            case 10:
                monologueTrigger.TriggerMonologue("OpenDay5");
                storyManager.Day5Update();
                break;
            case 11:
                monologueTrigger.TriggerMonologue("OpenDay5");
                storyManager.Day5Update();
                break;
            case 12:
                monologueTrigger.TriggerMonologue("OpenDay5");
                storyManager.Day5Update();
                break;
            case 13:
                monologueTrigger.TriggerMonologue("OpenDay5");
                storyManager.Day5Update();
                break;
        }
    }
    public void CheckEnding()
    {
        if (GameStats.Instance.CheckClear(GameStats.Instance.Stage))
        {
            if (GameStats.Instance.Stage == 5)
                GameStats.Instance.Stage = 7;
            else if (GameStats.Instance.Stage == 6)
                GameStats.Instance.Stage = 8;
            else if (GameStats.Instance.Stage == 4)
                GameStats.Instance.Stage = 6;
            else if (GameStats.Instance.Stage == 3)
                GameStats.Instance.Stage = 5;
            else if(GameStats.Instance.Stage == 8 || GameStats.Instance.Stage == 7)
            {
                switch(GameStats.Instance.Route)
                {
                    case 1:
                        GameStats.Instance.Stage = 9;
                        break;
                    case 2:
                        GameStats.Instance.Stage = 10;
                        break;
                    case 3:
                        GameStats.Instance.Stage = 11;
                        break;
                    case 4:
                        GameStats.Instance.Stage = 12;
                        break;
                    case 5:
                        GameStats.Instance.Stage = 13;
                        break;
                }
                GameStats.Instance.Route = 0;
            }
            else if(GameStats.Instance.Stage == 9)
            {
                switch (GameStats.Instance.Route)
                {
                    case 1:
                        GameStats.Instance.Stage = 13;
                        break;
                    case 2:
                        GameStats.Instance.Stage = 11;
                        break;
                }
                GameStats.Instance.Stage5CheckMemo = false;
                GameStats.Instance.Route = 0;
            }




            StartCoroutine(NextDay());
        }
    }
    private void Update()
    {
        if(!Monologue.activeSelf && !GalleryScreen.activeSelf && !MapScreen.activeSelf && !MessageScreen.activeSelf && !InternetScreen.activeSelf && !NoteScreen.activeSelf)
             CheckEnding();

        if (Input.GetKeyDown(KeyCode.Space))
            CallIncome("01012345678");
    }
}
