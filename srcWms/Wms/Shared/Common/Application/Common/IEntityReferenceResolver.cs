namespace Wms.Application.Common;

public interface IEntityReferenceResolver
{
    Task ResolveAsync(object entity, CancellationToken cancellationToken = default);
    Task ResolveManyAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default);
}
