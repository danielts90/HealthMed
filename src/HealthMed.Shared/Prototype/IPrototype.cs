namespace HealthMed.Shared.Prototype
{
    public interface IPrototype<T>
    {
        T ShallowClone();
        T DeepClone();
    }
}
