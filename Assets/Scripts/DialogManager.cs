using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    UIManager uIManager;
    /// <summary>
    /// 对话文本文件
    /// </summary>
    public TextAsset testAsset;
    /// <summary>
    /// 说话人名字Text
    /// </summary>
    public TMP_Text name_Text;
    /// <summary>
    /// 对话内容Text
    /// </summary>
    public TMP_Text content_Text;

    public Button next_Button;

    public GameObject buttonPrefab;
    public Transform buttonGroup;

    public int dialogIndex = 0;

    private string[] dialogRows;

    private void Awake()
    {
        ReadText(testAsset);
        uIManager = FindObjectOfType<UIManager>();
        
        
        next_Button.onClick.AddListener(OnClickNext);
    }

    void UpdateText(string name, string context)
    {
        name_Text.text = name;
        content_Text.text = context;
    }

    void ReadText(TextAsset textAsset)
    {
        dialogRows = textAsset.text.Split('\n');
        Debug.Log("读取成功！");
    }

    public void ShowDialogRow()
    {
        dialogPanel.SetActive(true);
        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');
            if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
            {
                next_Button.gameObject.SetActive(true);
                buttonGroup.gameObject.SetActive(false);
                UpdateText(cells[2], cells[4]);

                dialogIndex = int.Parse(cells[5]);
                return;
            }
            else if (cells[0] == "&" && int.Parse(cells[1]) == dialogIndex)
            {
                next_Button.gameObject.SetActive(false);
                buttonGroup.gameObject.SetActive(true);
                GenerateSelectButton(i);
            }
            else if (cells[0] == "END" && int.Parse(cells[1]) == dialogIndex)
            {
                dialogPanel.gameObject.SetActive(false);
                uIManager.OpenDefaultWindow();
                Debug.Log("对话结束");
            }
        }
    }

    void OnClickNext()
    {
        ShowDialogRow();
    }

    public void GenerateSelectButton(int index)
    {
        string[] cells = dialogRows[index].Split(',');
        if (cells[0] == "&")
        {
            GameObject button = Instantiate(buttonPrefab, buttonGroup);
            button.GetComponentInChildren<TMP_Text>().text = cells[4];
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnOperatorClick(int.Parse(cells[5]));
            });
            GenerateSelectButton(index + 1);
        }
    }

    void OnOperatorClick(int index)
    {
        dialogIndex = index;
        ShowDialogRow();

        for (int i = 0; i < buttonGroup.childCount; i++)
        {
            GameObject.Destroy(buttonGroup.GetChild(i).gameObject);
        }
    }
}
