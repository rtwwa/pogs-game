using UnityEngine;

public class Player : MonoBehaviour
{
    public Session gameSession;

    public void MakeMove()
    {
        gameSession.EndTurn();
    }
}
