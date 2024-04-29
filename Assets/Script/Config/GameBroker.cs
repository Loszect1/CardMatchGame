using System;

public class GameBroker : SingletonMono<GameBroker>
{
    public GameController gameController;

    public Action<int> onUpdateTurn = delegate { };
    public Action<int> onUpdateScore = delegate { };


    public void PlayGame()
    {
        gameController.Prepare();
    }
}
