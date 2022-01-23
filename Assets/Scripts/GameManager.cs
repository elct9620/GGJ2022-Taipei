using System;
using System.Collections.Generic;

public class GameManager
{
    /**
     * GameManager is Singleton to manager the Game
     */
    public static GameManager Instance = _instance ?? new GameManager();
    private static GameManager _instance;
    private GameManager()
    {
       // TODO
    }
}
