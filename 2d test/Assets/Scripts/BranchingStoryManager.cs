using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

[System.Serializable]
public class StoryNode
{
    public bool nextButton;
    public string text;
    public string[] options;
    public int[] nextNodeIds;
}

public class BranchingStoryManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text storyText;
    public Button[] optionButtons; 
    public Button nextButton;

    [Header("Story Nodes")]
    public StoryNode[] storyNodes;

    private int currentNode = 0;

    void Start()
    {
        DisplayNode(currentNode);
    }
    public void DisplayNode(int nodeIndex)
    {
        currentNode = nodeIndex;
        StoryNode node = storyNodes[nodeIndex];

        // Set story text
        storyText.text = node.text;

        if (node.nextButton)
        {
            // Hide all option buttons
            foreach (Button btn in optionButtons)
            {
                btn.gameObject.SetActive(false);
            }

            // Show nextButton
            nextButton.gameObject.SetActive(true);
            nextButton.onClick.RemoveAllListeners();

            if (node.nextNodeIds.Length > 0)
            {
                int nextNode = node.nextNodeIds[0]; // usually the first element
                nextButton.onClick.AddListener(() => DisplayNode(nextNode));
            }
        }
        else
        {
            // Hide nextButton
            nextButton.gameObject.SetActive(false);

            // Show option buttons
            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < node.options.Length)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    optionButtons[i].GetComponentInChildren<TMP_Text>().text = node.options[i];

                    int nextNode = node.nextNodeIds[i];
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => DisplayNode(nextNode));
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
