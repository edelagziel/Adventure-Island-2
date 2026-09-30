namespace AdventureIsland.Enemies
{
    public sealed class StaticSpiderEnemy : Enemy, IDefeatable, IDestructible
    {
        public bool TryDefeat() => TryDie();

        public bool TryDestroy() => TryDie();
    }
}
