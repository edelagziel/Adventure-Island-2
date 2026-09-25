public interface IAnimalBuilder<TAnimal>
    where TAnimal : Animal
{
    TAnimal Build();
}
