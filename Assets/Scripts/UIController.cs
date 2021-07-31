using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textScore;

    [SerializeField] TextMeshProUGUI textLastScore;

    [SerializeField] TextMeshProUGUI textBestScore;
    
    [Space]
    
    [SerializeField] RectTransform bestTransform;
    [SerializeField] RectTransform lastTransform;
    [SerializeField] Vector2 bestStartEnd;
    [SerializeField] Vector2 lastStartEnd;
    [SerializeField] float durationBest, durationLast;
   
    
    [Space]
    [SerializeField] Canvas menu;

    public void OnStart()
    {        
        if(BirdScripts.instance)
        {
            GoToStartPosition();
            textScore.enabled = true;
            textScore.text = "Score\n" + 0;
            BirdScripts.instance.Play();
        }
    }

    void SetPoints(int point)
    {
        textScore.text = "Score\n" + point;
    }

    int GetScore(string str)
    {
        int result = -1;

        foreach (var item in str.Split(new char[] { ' ', '\n' }))
        {
            if(Int32.TryParse(item, out result))
            {
                break;
            }
        }

        return result;
    }

    void GoToStartPosition()
    {
        Vector3 pos;

        pos = bestTransform.anchoredPosition;
        pos.x = bestStartEnd.x;
        bestTransform.anchoredPosition = pos;

        pos = lastTransform.anchoredPosition;
        pos.x = lastStartEnd.x;
        lastTransform.anchoredPosition = pos;
    }

    void ShowScores()
    {
        bestTransform.DOAnchorPosX(bestStartEnd.y, durationBest);
        lastTransform.DOAnchorPosX(lastStartEnd.y, durationLast);
    }


    public void SaveScore()
    {
        textScore.enabled = false;

        menu.enabled = true;

        ShowScores();

        int last, best;

        best = GetScore(textBestScore.text);
        last = BirdScripts.instance.pointCount;

        if (best < last)
        {
            best = last;
        }

        PlayerPrefs.SetInt("Last", last); 
        PlayerPrefs.SetInt("Best", best);
        
        textLastScore.text = "Last\n" + last;
        textBestScore.text = "Best\n" + best;
    }

    void LoadScore()
    {
        textLastScore.text = "Last\n" + (PlayerPrefs.HasKey("Last") ? PlayerPrefs.GetInt("Last") : 0);
        textBestScore.text = "Best\n" + (PlayerPrefs.HasKey("Best") ? PlayerPrefs.GetInt("Best") : 0);
    }

    void Start()
    {
        BirdScripts.OnSaveScore += SaveScore;
        BirdScripts.OnSetPoints += SetPoints;

        LoadScore();
        ShowScores();
    }
}
