using API.Entities;
using Core.Specification;

public class PostSpecification : BaseSpecification<PostForum>
{
    public PostSpecification(OfertySpecParams postyParams)
    : base(x =>
        (string.IsNullOrEmpty(postyParams.Search) || x.Tytul.ToLower().Contains(postyParams.Search)))
    {

    }
}