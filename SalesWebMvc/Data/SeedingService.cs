using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using SalesWebMvc.Models.Enums;

namespace SalesWebMvc.Data
{
    public class SeedingService
    {
        private SalesWebMvcContext _context;
    //fazer um construtor para injeção de dependencia
    public SeedingService(SalesWebMvcContext context)
        {
            _context = context;
        }
        //esta ação (operação) será responsavel por popular a base de dados.
        public void Seed()
        {
            //este if faz o text para verificar se o banco de dados já foi populado.
            if (_context.Department.Any() ||
                 _context.Seller.Any() ||
                 _context.SalesRecord.Any())
            {
                return;
            }
            //neste momento eu vou popular meu banco de dados. na Coluna Departamento.
            Department d1 = new Department(1, "Computers");
            Department d2 = new Department(2, "Electronics");
            Department d3 = new Department(3, "Fashion");
            Department d4 = new Department(4, "Books");

            //populando seller vendendor.
            Seller s1 = new Seller(1, "Bob Brown", "bob@gmail.com", new DateTime(1998, 4, 21), 1000.0, d1);
            Seller s2 = new Seller(2, "Maria Green", "maria@gmail.com", new DateTime(1979, 12, 31), 3500.0, d2);
            Seller s3 = new Seller(3, "Alex Grey", "alex@gmail.com", new DateTime(1988, 1, 15), 2200.0, d1);
            Seller s4 = new Seller(4, "Marta Red", "martha@gmail.com", new DateTime(1993, 11, 30), 3000.0, d4);
            Seller s5 = new Seller(5, "Donald Blue", "donald@gmail.com", new DateTime(2000, 1, 9), 4000.0, d3);
            Seller s6 = new Seller(6, "Alex Pink", "bob@gmail.com", new DateTime(1997, 3, 4), 3000.0, d2);

            SalesRecord r1 = new SalesRecord(1, new DateTime(2018, 10, 25), 11000.0, SaleStatus.Billed, s1);
            SalesRecord r2 = new SalesRecord(2, new DateTime(2018, 10, 26), 12000.0, SaleStatus.Billed, s2);
            SalesRecord r3 = new SalesRecord(3, new DateTime(2018, 10, 27), 13000.0, SaleStatus.Billed, s3);
            SalesRecord r4 = new SalesRecord(4, new DateTime(2018, 10, 28), 14000.0, SaleStatus.Billed, s4);
            SalesRecord r5 = new SalesRecord(5, new DateTime(2018, 10, 29), 15000.0, SaleStatus.Billed, s5);
            SalesRecord r6 = new SalesRecord(6, new DateTime(2018, 10, 30), 16000.0, SaleStatus.Billed, s6);
            SalesRecord r7 = new SalesRecord(7, new DateTime(2018, 10, 31), 17000.0, SaleStatus.Billed, s6);
            SalesRecord r8 = new SalesRecord(8, new DateTime(2018, 10, 1),  18000.0, SaleStatus.Billed, s6);
            SalesRecord r9 = new SalesRecord(9, new DateTime(2018, 10, 2),  19000.0, SaleStatus.Billed, s6);
            SalesRecord r10 = new SalesRecord(10, new DateTime(2018, 10, 3), 20000.0, SaleStatus.Billed, s1);


            //agora vamos adicionar os dados no banco de dados. Esta ação AddRange permite que adicione varios objetos de uma só vez.
            _context.Department.AddRange(d1, d2, d3, d4);

            _context.Seller.AddRange(s1, s2, s3, s4, s5, s6);

            _context.SalesRecord.AddRange(
                r1, r2, r3, r4,
                r5, r6, r7, r8,
                r9, r10
                );

            //para salvar e confirmar as alterações no bd
            _context.SaveChanges();
        }
    }
}
