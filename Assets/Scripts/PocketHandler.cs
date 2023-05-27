using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketHandler : MonoBehaviour
{

    public static PocketHandler Instance;
    public static System.Action OnScore;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Striker")
        {
            other.GetComponent<Rigidbody2D>().drag = 5;
        }
        if (other.tag != "Striker")
        {
            other.gameObject.SetActive(false);
            OnScore?.Invoke();
        }
        if (other.tag == "BlackPuck")
        {
            ScoreManager.Instance.AddBlackPuck(1, GameManager.Instance.CurrentTurn);
        }
        if (other.tag == "WhitePuck")
        {
            ScoreManager.Instance.AddWhitePuck(1, GameManager.Instance.CurrentTurn);
        }
        if (other.tag == "Queen")
        {
            ScoreManager.Instance.AddQueen(1, GameManager.Instance.CurrentTurn);
        }

    }
}
