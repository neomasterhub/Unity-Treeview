using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Slider : MonoBehaviour
{
    public List<GameObject> treeviews;
    private int index = 0;
    private int maxIndex = 0;
    private Button nextButton;
    private Button previousButton;
    private Text header;
    private Text log;

    private void Awake()
    {
        Debug.ClearDeveloperConsole();

        header = gameObject.transform.Find("DemoName").GetComponent<Text>();
        header.text = treeviews.Any() ? treeviews[0].name : "";

        previousButton = gameObject.transform.Find("Previous").GetComponent<Button>();
        previousButton.onClick.AddListener(PreviousButtonClick);

        nextButton = gameObject.transform.Find("Next").GetComponent<Button>();
        nextButton.onClick.AddListener(NextButtonClick);
        
        log = gameObject.transform.Find("Log").GetComponent<Text>();
        log.text = "";
        
        maxIndex = treeviews.Count - 1;
        
        if (treeviews.Count > 1)
        {
            for (int i = 1; i < treeviews.Count; i++)
            {
                treeviews[i].SetActive(false);
            }
        }
    }

    private void NextButtonClick()
    {
        if (!treeviews.Any())
        {
            return;
        }

        log.text = "";
        treeviews[index].SetActive(false);
        index = index < maxIndex ? index + 1 : 0;
        treeviews[index].SetActive(true);
        header.text = treeviews[index].name;
    }

    private void PreviousButtonClick()
    {
        if (!treeviews.Any())
        {
            return;
        }

        log.text = "";
        treeviews[index].SetActive(false);
        index = index == 0 ? maxIndex : index - 1;
        treeviews[index].SetActive(true);
        header.text = treeviews[index].name;
    }
}
