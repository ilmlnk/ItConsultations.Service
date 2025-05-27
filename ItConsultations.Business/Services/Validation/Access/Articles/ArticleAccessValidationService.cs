using ItConsultations.Business.Entities.ErrorObject;

namespace ItConsultations.Business.Services.Validation.Access.Articles;

public class ArticleAccessValidationService : ValidationService, IArticleAccessValidationService
{
    public void ValidateAccessToModify(long id)
    {
        /*Expect(() => id, new ErrorObject());

        Validate();*/
    }

    public void ValidateAccessToDelete(long id)
    {

    }

    public void ValidateAccessToGet(long id)
    {
        throw new NotImplementedException();
    }

    public void ValidateAccessToAdd(long id)
    {
        throw new NotImplementedException();
    }
}
