namespace Wms.Application.Common;

public interface IDocumentReferenceReadEnricher
{
    Task EnrichHeadersAsync<T>(IList<T> items, CancellationToken cancellationToken = default) where T : class;
    Task EnrichLinesAsync<T>(IList<T> items, CancellationToken cancellationToken = default) where T : class;
    Task EnrichImportLinesAsync<T>(IList<T> items, CancellationToken cancellationToken = default) where T : class;
    Task EnrichLineSerialsAsync<T>(IList<T> items, CancellationToken cancellationToken = default) where T : class;
    Task EnrichRoutesAsync<T>(IList<T> items, CancellationToken cancellationToken = default) where T : class;
}
