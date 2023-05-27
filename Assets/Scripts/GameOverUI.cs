using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private TMPro.TextMeshProUGUI winnerNameText;
    [SerializeField] private Button retryButton;

    void Start()
    {
        retryButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
        GameManager.Instance.OnGameOver += Show;
        container.SetActive(false);
    }


    void Show()
    {
        container.gameObject.SetActive(true);
        winnerNameText.text = ScoreManager.Instance.WinnerNameDecider();
    }
}
