using System.Reflection.Metadata.Ecma335;

namespace WebApi.Models.Pagination;

public class PaginationParams
{
    private int itemsPerPage;
    private const int _maxItemsPerPage = 50;
    public int Page { get; set; } = 1;

    public int ItemPerPage
    {
        get => itemsPerPage;
        set => itemsPerPage = value > _maxItemsPerPage ? _maxItemsPerPage : value;
    }
}