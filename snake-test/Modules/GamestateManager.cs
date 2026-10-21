namespace snake_test;

public static partial class GamestateManager
{
    private static readonly List<Gamestate> states = new();
    private static Gamestate? currentState = null;
    private static string defaultState = "game";

    public static void InitStates()
    {
        states.Clear();

        AddState(new Game());
        AddState(new Death());

        ChangeState(defaultState);
    }

    public static void AddState(Gamestate state)
    {
        states.Add(state);
    }

    public static void Update()
    {
        if (currentState == null) return;
        currentState.Update();
    }

    public static void ChangeState(string name)
    {
        Gamestate? nextState = FindState(name);
        if (nextState == null) return;

        currentState?.OnExit();
        currentState = nextState;
        currentState.OnEnter();
    }

    public static Gamestate? FindState(string name)
    {
        return states.FirstOrDefault(s => s.name == name);
    }
}