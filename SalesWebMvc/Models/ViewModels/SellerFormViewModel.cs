
using System.Collections.Generic;


namespace SalesWebMvc.Models.ViewModels
{
    //esta é a classe que vai conter os dados para formulário de cadastro de vendedor
    public class SellerFormViewModel
    {
        //quais são os dados necessários para uma tela de cadastro de vendedor
        public Seller Seller  { get; set; }
        //caixinha para colocar a lista de vendedores..esta lista vem de Departments.
        public ICollection<Department> Departments { get; set; }

    }
}
