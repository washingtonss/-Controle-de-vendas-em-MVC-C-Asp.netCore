using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Department
    {
        //quando digito prop tt cria a propriedade
        //classe basica do departamento
        public int Id { get; set; }
        public string Name { get; set; }
        //implementar com ICollection com a pagina (Sellers) vendedores.
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        //temos um construtor vazio como padrão. Lembrando o construtor vazio e criado porque temos o construtor com argumento. Se não houvesse o com argumentos não teria a necessidade
        public Department()
        {

        }
        //agora crio o construtor com argumentos. Mas não coloco os atributos que tiver coleções (ICollection)
        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        //adicionar vendedores. 
        //simples vou na minha ICollection pego a lista de vendores Sellers e adciono neste meu vendedor seller é o meu parametro.
        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        //vamos calcular inicial e final das vendas neste departamento.
        public double TotalSales (DateTime initial, DateTime final)
        {
            //ná expressão lambda
            //(1º seller (vendedor) pego cada vendedor da minha lista => chamo o total de (TotalSales)vendas do vendedor(no periodo inicial e final) )
           //logo eu por fim faço a soma (Sum) deste resultado aos vendedores(Sellers) deste departamento.
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }

    }
}
