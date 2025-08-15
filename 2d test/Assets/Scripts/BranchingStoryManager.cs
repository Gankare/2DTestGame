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
    public TMP_Text storyText;
    public Button[] optionButtons;
    public Button nextButton;
    public StoryNode[] storyNodes;

    private int currentNode = 0;
    private AutoTypewriterTMP storyTyper;

    private StoryNode pendingNode;

    void Start()
    {
        storyTyper = storyText.GetComponent<AutoTypewriterTMP>();
        DisplayNode(currentNode);
    }

    public void DisplayNode(int nodeIndex)
    {
        currentNode = nodeIndex;
        StoryNode node = storyNodes[nodeIndex];
        pendingNode = node;

        // Set story text
        storyText.text = node.text;

        // Disable all buttons while typing
        foreach (Button btn in optionButtons)
            btn.interactable = false;
        nextButton.interactable = false;

        // Remove old event subscription to prevent stacking
        storyTyper.OnTypingComplete -= OnTypingFinished;
        storyTyper.OnTypingComplete += OnTypingFinished;
    }

    private void OnTypingFinished()
    {
        if (pendingNode == null)
            return;

        if (pendingNode.nextButton)
        {
            // Hide option buttons
            foreach (Button btn in optionButtons)
                btn.gameObject.SetActive(false);

            // Show and enable next button
            nextButton.gameObject.SetActive(true);
            nextButton.interactable = true;
            nextButton.onClick.RemoveAllListeners();

            if (pendingNode.nextNodeIds.Length > 0)
            {
                int nextNode = pendingNode.nextNodeIds[0];
                nextButton.onClick.AddListener(() => ClickButtonAndGo(nextNode));
            }
        }
        else
        {
            // Hide next button
            nextButton.gameObject.SetActive(false);

            // Show and enable option buttons
            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < pendingNode.options.Length)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    optionButtons[i].GetComponentInChildren<TMP_Text>().text = pendingNode.options[i];
                    optionButtons[i].interactable = true;

                    int nextNode = pendingNode.nextNodeIds[i];
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => ClickButtonAndGo(nextNode));
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }
        }

        pendingNode = null;
    }

    // Hides all buttons immediately and then moves to the next node
    private void ClickButtonAndGo(int nextNode)
    {
        // Hide all option buttons
        foreach (Button btn in optionButtons)
            btn.gameObject.SetActive(false);

        // Hide next button
        nextButton.gameObject.SetActive(false);

        // Display next node
        DisplayNode(nextNode);
    }
}
