namespace Scripts.Animations
{
    public interface IAnimatorStateListener
    {
        public void OnStateEnter(int hash);
        public void OnStateExit(int hash);
    }
}