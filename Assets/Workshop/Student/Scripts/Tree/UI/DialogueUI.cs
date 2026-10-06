using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI npcText;
    public Transform choiceContainer;
    public Button choiceButtonPrefab; // ลาก Prefab ปุ่มตัวเลือกมาใส่
    public GameObject closeButtonDialogue;
    private DialogueSequen InteractNpcSequen;

    // เก็บปุ่มที่ถูกสร้างขึ้น เพื่อนำไปทำลาย/ซ่อนในภายหลัง
    private List<Button> activeButtons = new List<Button>();

    public void Setup(DialogueSequen sequen)
    {
        //1. Set Dialogue Sequen
        this.InteractNpcSequen = sequen;
        DialogueNode currentNode = InteractNpcSequen.tree.root;
        ShowDialogue(currentNode);
        //Show UI
        dialoguePanel.SetActive(true);
        gameObject.SetActive(true);
        closeButtonDialogue.SetActive(false);
    }

    public void ShowDialogue(DialogueNode node)
    {
        // 2. set ให้เป็น โหนดปัจจุบัน
        InteractNpcSequen.currentNode = node;
        // 3. แสดงข้อความของ NPC
        npcText.text = node.text;
        // 4. ล้างปุ่มตัวเลือกเก่า
        ClearChoices();

        // 5. สร้างปุ่มตัวเลือกใหม่ตาม nexts
        var chios = new List<string>(node.nexts.Keys);
        for (int i = 0; i < chios.Count; i++)
        {
            string choiceText = chios[i];
            CreateChoiceButton(choiceText, i);
        }

    }

    private void CreateChoiceButton(string text, int index)
    {
        Button newButton = Instantiate(choiceButtonPrefab, choiceContainer);

        
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = text;

        
        newButton.onClick.AddListener(() => OnChoiceSelected(index));

        activeButtons.Add(newButton);
    }

    private void ClearChoices()
    {
        foreach (Button button in activeButtons)
        {
            Destroy(button.gameObject);
        }
        activeButtons.Clear();
    }

    private void OnChoiceSelected(int index)
    {
        
        InteractNpcSequen.SelectChoice(index);
    }
    public void ShowCloseButtonDialog()
    {
        closeButtonDialogue.gameObject.SetActive(true);
    }
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        ClearChoices();
    }
}