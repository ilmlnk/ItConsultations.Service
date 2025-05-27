namespace ItConsultations.Business.Services.Validation.Access.Articles;

public interface IArticleAccessValidationService
{
    void ValidateAccessToModify(long id);

    void ValidateAccessToDelete(long id);

    void ValidateAccessToGet(long id);

    void ValidateAccessToAdd(long id);
}
