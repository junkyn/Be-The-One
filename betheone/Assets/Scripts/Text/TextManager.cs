using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TextManager : MonoBehaviour
{
    [SerializeField] GameObject background;
    [SerializeField] AudioSource audioSource;

    [SerializeField] TextMeshProUGUI textField;
    [SerializeField] TextMeshProUGUI arrow;
    [SerializeField] float timeForCharacter;
    float characterTime;

    bool wait = true;
    bool isTypingEnd = false;
    float timer;
    bool isTyping = false;
    int index = 0;

    string[] str;
    char[] chars;

    void OnEnable()
    {
        index = 0;
        textField.text = "";
        switch (GameStats.Stage)
        {
            case 0:
                str = new string[] {
                    ".....",
                    "����? �Ӹ��� ���� �͸� ����.",
                    "�ƹ��͵� ����� ���� �ʾ�.",
                    "...",
                    "�ٸ��� ������ �� ����.",
                    "��ο� �� ħ�뿡 �����ִ� �� ������ �տ� �ִ� �޴����� �����ϴ� �� �����",
                    "�ƹ��͵� �� �� ���°ǰ�?"
                };
                break;
            case 1:
                str = new string[] {
                    "������ �� �޴����� ������ ���� �� ����.",
                    "�켱 ���� ���������� �˾ƺ���?"
                };
                break;
            case 2:
                str = new string[]
                {
                    "������?.. �̰� �� �̸��ΰǰ�?"
                }
                ; break;
        }
        index = 0;
        isTyping = false;
        isTypingEnd = true;
        timer = timeForCharacter;
        characterTime = timeForCharacter;
    }

    void Update()
    {
        if (!isTyping&&isTypingEnd)
        {
            isTyping = true;
            chars = str[index].ToCharArray();
            StartCoroutine(Typer(chars, textField));
        }
        if (isTyping&&isTypingEnd&&wait)
        {
            wait = false;
            StartCoroutine(GoNext());
        }
    }

    IEnumerator GoNext()
    {
        Debug.Log(index);
        float k = 0;
        bool turn = true;
        while (!Input.GetMouseButtonDown(0))
        {
            if (Input.GetMouseButtonDown(0))
                break;
            if (turn)
            {
                k += 0.01f;
                yield return new WaitForSeconds(0.2f*Time.deltaTime);
                if (k >= 1f)
                    turn = false;
            }
            else
            {
                k -= 0.01f;
                yield return new WaitForSeconds(0.2f * Time.deltaTime);
                if (k <= 0f)
                    turn = true;
            }
            arrow.color = new Color(1f, 1f, 1f, k);
        }
        arrow.color = new Color(1f, 1f, 1f, 0);
        index++;
        if(index == str.Length)
        {
            textField.text = "";
            isTyping = false;
            wait = true;
            GameStats.Stage++;
            background.SetActive(false);
        }
        textField.text = "";
        isTyping = false;
        wait = true;
    }

    IEnumerator Typer(char[] chars, TextMeshProUGUI textObj)
    {
        int currentChar = 0;
        int charLength = chars.Length;
        isTypingEnd = false;
        while (currentChar < charLength)
        {
                if (timer >= 0)
                {
                    yield return null;
                    timer -= Time.deltaTime;
                }
                else
                {
                    audioSource.Stop();
                    audioSource.Play();
                    textObj.text += chars[currentChar].ToString();
                    currentChar++;
                    timer = characterTime;
                }
        }
        if (currentChar >= charLength)
        {
            isTypingEnd = true;
            yield break;
        }
    }
}
