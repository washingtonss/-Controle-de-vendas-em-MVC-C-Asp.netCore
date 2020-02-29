//using SalesWebMvc.Data;
//using SalesWebMvc.Data;

using SalesWebMvc.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Service
{
    public class DepartmentService
    {
        //trouxe estás dependencias do SellerService e injetei as dependencias logo abaixo
        private readonly SalesWebMvcContext _context;

        public DepartmentService (SalesWebMvcContext context)
        {
            _context = context;
        }
        //criar um método para retornar todos os departamentos
        public async Task<List<Department>> FindAllAsync()
        {
            //estou criando o retorno da minha lista para me retornar ordenada por nome expressão lambda no OrderBy(x => x.Name)
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
