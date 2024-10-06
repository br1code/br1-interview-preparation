namespace Br1InterviewPreparation.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
        : base()
    {
    }

    public NotFoundException(string name, Guid id)
        : base($"Entity {name} with Id {id} was not found.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }
}
