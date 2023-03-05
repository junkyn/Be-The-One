using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] StoryManager storyManager;
    public List<Chat> chats;

    public void TriggerChat(string name)
    {
        for (int i = 0; i < chats.Count; i++)
            if (chats[i].name.Equals(name))
            {
                StartCoroutine(gameManager.MessageChatUpdate(chats[i]));

                if(name.Equals("������") && GameStats.Instance.Stage.Equals(1))
                {
                    storyManager.OpenFirstJiHye();
                }
                if(name.Equals("������")&& GameStats.Instance.Stage.Equals(3))
                {
                    storyManager.Stage3OpenJiHye();
                }
                if (name.Equals("�Ǳ� ���ź���") && (GameStats.Instance.Stage.Equals(7) || GameStats.Instance.Stage.Equals(8)))
                {
                    if(!GameStats.Instance.Stage4CheckMessage)
                        storyManager.Day4CheckMessage();
                }
            }
    }
    public void Stage3JiHye()
    {
        for(int i = 0; i<chats.Count; i++)
        {
            if (chats[i].name.Equals("������"))
            {
                chats[i].sentences.Add("-----------------------\n\n�̾� ���� �Ͼ�� ���� �������̾�?\n\n-----------------------");
                chats[i].sentences.Add("-----------------------\n\n�ƴϾ� �츮 �ٵ� ���� ����?\n\n-----------------------");
                chats[i].sentences.Add("-----------------------\n\n��... ���� 11�� �?\n\n-----------------------");
                chats[i].sentences.Add("-----------------------\n\n�˾Ҿ� �׷� ���� ��~\n\n-----------------------");
                StartCoroutine(gameManager.MessageChatUpdate(chats[i]));
            }
        }
    }
    public void Stage4Ashylum()
    {
        for (int i = 0; i < chats.Count; i++)
        {
            if (chats[i].name.Equals("�Ǳ� ���ź���"))
            {
                chats[i].sentences.Add("-----------------------\n\n ������ ���ؼ����� ������ �ظ��� ��ü�� ����� Ư�� ���̽��� �з��� �� �����ϴ�.\n" +
                    "���� ���� �� Ȱ���� �޶� ���� ������� ���ϴ� ������ ���� �� �ְ�,\n��ü ��ְ� �߻��� ���ɼ��� �ֽ��ϴ�.\n" +
                    "���� �� ������ �ִٸ� �޸� ���� �����ϴ� ���� ��õ�帮��\n���� Ȱ���� ��������� �ڿ������� ġ�� �� ������ �����˴ϴ�.\n"
                    + "�� �ٸ� �ñ����̳� �������� �ִٸ� ��ȭ ����̳�, �ٽ� �湮���ֽñ� �ٶ��ϴ�.\n�����մϴ�.\n\n-----------------------");
            }
        }
    }
}
