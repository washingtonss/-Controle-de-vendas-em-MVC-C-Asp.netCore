using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome ")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Tamanho do nome dever ser entre 03 e 60 caracter...")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Coloque o Email...")]
        [EmailAddress(ErrorMessage = "Entre com email válido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Data de Nascimento  é Obrigátoria")]
        [Display(Name= "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Valor de salário")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} deve ser no  minimo {1} e no maximo {2}")]
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }

        public Department Department { get; set; } //A classe Seller(vendedor) tem um departamento. Eu estou dizendo isto
        public int DepartmentId { get; set; }//aqui eu coloco o mesmo nome da classe de DepartmentId mas com Id maaiusculo para garantir que tenha Id

        //ICollection é usado para fazer a conexão com a pagina <SalesRecord> onde a propriedade Sales(vendas) recebe a<SalesRecord> Record Vendas
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        //criamos o construtorvazio.
        public Seller()
        {
           
        }
        //Estou criando o meu construtor com argumentos. Mais um lembre atributos que são colleções não construimos exemplos Sales (Vendas) Logo acima.
        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }
        //criação da operação(ação) adicionar records vendas em vendedores.
        public void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }
        //remover record vendas
        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }
        //1-está operação vai retornar um total de vendas deste vendedor neste periodo data-inicial e data-final.
        //2-estou usando double porque vai me retornar o total da operação. 
        //3-vamos fazer está operação usando o link.
        public double TotalSales (DateTime initial, DateTime final)
        {
            //1-este return Sales(vendedores) é a lista de vendas que está vindo da linha 17 da coleção ICollection
            //2-vou filtrar a lista de vendas com Where para receber as vendas em uma determinada data de inicio e fim
            //3-pegarei todo objeto sr sque seja >=(menor ou igual a data inicial) && sr. <=(menor ou igual a data final)
            //4-vou fazer agora a soma das vendas .Sum(sr => sr.Amount); Sum para somar sr(vendas).Amount(montante) neste periodo
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount); //Simples usamos o link. com expressão lambda.
        }
    }
}
