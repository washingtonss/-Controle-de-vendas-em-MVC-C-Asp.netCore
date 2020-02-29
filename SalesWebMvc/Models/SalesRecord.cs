using SalesWebMvc.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMvc.Models
{
    public class SalesRecord
    {
        //declarando as propriedades.
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double Amount { get; set; }

        public SaleStatus Status { get; set; } //Esta classe esta me trazendo os atributos pendencia, faturada e cancelada
        //mostrando que um vendedoe pode ter varios records

        public Seller Seller { get; set; }//Olha que interressante já nesta classe SalesRecord Record Vendas eu vou dizer cadas recorde vendas possui um vendedor

        //vamos criar o construtor vazio padrão.
        public SalesRecord()
        {

        }
        //temos construtor com argumentos. Mais um lembre atributos que são colleções não construimos. Aqui não temos nenhum
        public SalesRecord(int id, DateTime date, double amount, SaleStatus status, Seller seller)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
        }
    }
}
