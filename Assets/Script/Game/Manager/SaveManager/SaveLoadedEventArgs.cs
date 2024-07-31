using System;

public class SaveLoadedEventArgs : EventArgs
{
    public SaveLoadedEventArgs(GameData gameData)
    {
        this.GameData = gameData;
    }

    public GameData GameData;
}