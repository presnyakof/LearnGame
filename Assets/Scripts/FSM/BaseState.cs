namespace LearnGame.FSM
{
    public abstract class BaseState
    {
        public string StateID = "base";
        public abstract void Execute();

        public string GetID()
        {
            return StateID;
        }
    }
}
