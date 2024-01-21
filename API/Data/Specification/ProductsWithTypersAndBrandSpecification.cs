

using API.Entities;

namespace Core.Specification
{
    public class ProductsWithTypersAndBrandSpecification : BaseSpecification<Oferta>
    {
        public ProductsWithTypersAndBrandSpecification(OfertySpecParams ofertyParams)
        : base(x=>
             (string.IsNullOrEmpty(ofertyParams.Search)||x.Tytul.ToLower().Contains(ofertyParams.Search))&&
            (string.IsNullOrEmpty(ofertyParams.type)|| x.Typ==ofertyParams.type)
        )
        {
            
            AddOrderByDescending(x=>x.DataDodaniaOferty);
            ApplyPaging(ofertyParams.PageSize * (ofertyParams.PageIndex -1),ofertyParams.PageSize);

            if(!string.IsNullOrEmpty(ofertyParams.Sort)){
                switch(ofertyParams.Sort){
                    case "priceAsc":
                        AddOrderBy(p=>p.Cena);
                    break;
                    case "priceDesc":
                        AddOrderByDescending(p=>p.Cena);
                        break;
                    default:
                        AddOrderBy(n=>n.DataDodaniaOferty);
                        break;
                }
            }
        }

       
    }
}