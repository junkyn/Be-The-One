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
}
