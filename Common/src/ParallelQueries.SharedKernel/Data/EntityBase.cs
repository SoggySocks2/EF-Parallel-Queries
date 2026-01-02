using ParallelQueries.SharedKernel.Contracts;

namespace ParallelQueries.SharedKernel.Data;

public abstract class EntityBase : IEntity<Guid>
{
    public Guid Id { get; protected set; }
    public DateTime Created { get; protected set; }
}
