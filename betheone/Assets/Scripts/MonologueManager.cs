using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MonologueManager : MonoBehaviour
{
    public Queue<string> sentences;

    [SerializeField] MonologueTrigger monologueTrigger;
    [SerializeField] TextMeshProUGUI monologueText;

    public float typeSpeed;
    WaitForSeconds typeDelay;
    bool typing;
    string currentSentence;
    [SerializeField] TextMeshProUGUI arrow;

    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        sentences = new Queue<string>();

        TypeSpeedUpdate();
        arrow.DOColor(new Color(1, 1, 1, 0), .15f).SetLoops(-1, LoopType.Yoyo);
    }

    //Ÿ���� �ӵ��� ������Ʈ�մϴ�.
    public void TypeSpeedUpdate()
    {
        typeDelay = new WaitForSeconds(typeSpeed);
    }

    private void Update()
    {
        //Ŭ���Ͽ� ���� �������� �Ѿ�ϴ�.
        //������ Ÿ���εǰ� �ִٸ�, Ÿ������ ��ŵ�մϴ�.
        if (Input.GetMouseButtonDown(0))
        {
            if (!typing)
            DisplayNextSentence();
            else
            {
                monologueText.text = currentSentence;
                StopAllCoroutines();
                EndTyping();
            }
        }
    }

    //������ �����մϴ�.
    public void StartMonologue(Monologue monologue)
    {
        Debug.Log(monologue.stage);
        monologueTrigger.gameObject.SetActive(true);
        sentences.Clear();

        foreach (string sentence in monologue.sentences)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    //���� �������� �Ѿ�ϴ�.
    public void DisplayNextSentence()
    {
        if (sentences.Count.Equals(0))
        {
            EndMonologue();
            return;
        }

        string sentence = sentences.Dequeue();
        currentSentence = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    //������ Ÿ�����մϴ�.
    IEnumerator TypeSentence(string sentence)
    {
        monologueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            typing = true;
            arrow.gameObject.SetActive(false);

            audioSource.Stop();
            audioSource.Play();

            monologueText.text += letter;
            yield return typeDelay;

            if (monologueText.text.Equals(sentence))
            {
                EndTyping();
            }
        }
    }
    
    //Ÿ������ �����մϴ�.
    void EndTyping()
    {
        typing = false;
        arrow.gameObject.SetActive(true);
    }

    //������ �����մϴ�.
    void EndMonologue()
    {
        monologueTrigger.gameObject.SetActive(false);
    }

}
