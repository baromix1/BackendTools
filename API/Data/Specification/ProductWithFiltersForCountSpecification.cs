using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace Core.Specification
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Oferta>
    {
        public ProductWithFiltersForCountSpecification(OfertySpecParams ofertyParams)
        : base(x=>
            (string.IsNullOrEmpty(ofertyParams.Search)||x.Tytul.ToLower().Contains(ofertyParams.Search))&&
            (string.IsNullOrEmpty(ofertyParams.type)|| x.Typ==ofertyParams.type)
            
        )
        {

        }
    }
}