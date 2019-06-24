namespace RDI.Domain.ApplicationCore.Interfaces
{
    public interface IAbsoluteDifferenceService
    {
        int[] Find(int[] inputArray, int refValue = 5);
    }
}
