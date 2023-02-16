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

    [Header("Lock Screen")]
    [SerializeField] RectTransform screenLocked, screenMain;
    [SerializeField] GameObject clickBlock;

    [SerializeField] Image iconLock;
    [SerializeField] Sprite spriteUnlock;
    [SerializeField] TextMeshProUGUI touchToUnlock;

    [Header("Game Stats")]
    int _date;
    int month = 7;
    string[] daylist = { "��", "ȭ", "��", "��", "��", "��", "��" };
    string day;
    [SerializeField] TextMeshProUGUI dateText, dayText;

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

    [Header("Setting")]
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource fx;
    [SerializeField] Slider bgmSlider, fxSlider;
    [SerializeField] Toggle slowToggle, fastToggle;

    void Start()
    {
        touchToUnlock.DOColor(transparent, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        DateSet();

        monologueTrigger.TriggerMonologue("Intro");

        screenPos = screenMain.position;
        appDelay = new WaitForSeconds(.55f);

        galleryZoomedPos = galleryZoomed.transform.position;
    }

    //��¥�� ������Ʈ�մϴ�.
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

    public void Unlock()
    {
        StartCoroutine(UnlockCoroutine());
    }

    //����� �����մϴ�.
    IEnumerator UnlockCoroutine()
    {
        iconLock.sprite = spriteUnlock;

        yield return new WaitForSeconds(.3f);

        screenLocked.DOPivotY(-.5f, 0);
        screenLocked.DOMove(screenPos, .5f);
        screenMain.gameObject.SetActive(true);

        monologueTrigger.TriggerMonologue("Unlocked");

        yield return appDelay;
        screenLocked.gameObject.SetActive(false);
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

    void Update()
    {
        
    }
}
