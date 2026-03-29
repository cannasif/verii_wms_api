namespace WMS_WEBAPI.Interfaces
{
    public interface IRequestCancellationAccessor
    {
        CancellationToken Get(CancellationToken cancellationToken = default);
    }
}
