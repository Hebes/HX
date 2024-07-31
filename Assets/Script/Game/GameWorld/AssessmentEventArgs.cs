using System;

public class AssessmentEventArgs : EventArgs
{
    public AssessmentEventArgs(AssessmentEventArgs.EventType type)
    {
        Type = type;
    }

    public readonly AssessmentEventArgs.EventType Type;

    public enum EventType
    {
        AddFlashTime,
        CurrentComboFinish,
        ContinueGame
    }
}