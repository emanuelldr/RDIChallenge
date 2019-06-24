namespace RDI.Domain.ApplicationCore.Interfaces
{
    public interface IRotationService
    {
        int[] Rotate(int[] inputArray, int rotations);
    }
}
