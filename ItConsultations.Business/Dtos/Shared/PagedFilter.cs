namespace ItConsultations.Business.Dtos.Shared;

public class PagedFilter
{
    public bool Ascending { get; set; }

    public string OrderBy { get; set; }

    public int? PageSize { get; set; }

    public int Skip { get; set; }
}
