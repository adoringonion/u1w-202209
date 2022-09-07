namespace DefaultNamespace
{
    public class Poop
    {
        public float Volume
        {
            get;
        }

        public Poop(float volume)
        {
            Volume = volume;
        }

        public Poop Add(Poop poop)
        {
            return new Poop(Volume + poop.Volume);
        }

        public ScoreManager.Score ToScore()
        {
            return new ScoreManager.Score(Volume);
        }
    }
}