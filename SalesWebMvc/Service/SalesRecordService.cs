using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Service
{
    public class SalesRecordService
    {
        //vamos inserir as dependencias em 1º lugar
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }
        //vamos encontrar as Sales (vendas) neste intervalo de data
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            //fazendo mensão ao sql banco de dados. mas com C#
            //este objeto de consulta foi construido com o link
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                //estou pegando o result que já estava fazendo uma consulta (filtro) e aplicando outra expressão lambda  Where(x => x.Date >= minDate.Value); buscando a data que seja maior ou igual a data minima 
                result = result.Where(x => x.Date >= minDate.Value);
            }
            //vamos fazer a data maxima é mesmo caminho usando reuslt que já vem da consulta em link expressão lambda. logo faço um filtro em baixo
            //HasValue esta dizendo foi informado uma data maxima
            if (maxDate.HasValue)
            {
                //fará uma busca no bd e trará tudo que for menor ou igual a data maxima que o usuario passou.
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            //Somente isto já me traria a lista. mas quis fazer algumas explorações no banco de dados e sem usar (sql) apenas expressões lambdas e link
            //return result.ToList();

            //vou fazer um join com a tabela de vendedor e departamento
            return await result
                //O resulmo das linhas abaixos é que eu faço um Join entre Vendedor e Depertamento e depois ainda ordenar  decrescentemente por datas
                //fazer os Join das tabelas (vendedor)
                .Include(x => x.Seller)
                //neste momento eu faço o join(junção) com a tabela Seller(vendedor) e Department(Departamento)
                .Include(x => x.Seller.Department)
                //agora por ultimo ou vou fazer o OrderByDescending uso a expresão lambda e vou ordenar por data.
                .OrderByDescending(x => x.Date)
                //mostrando a lista
                .ToListAsync();
        }

        //implementando a busca simples
        //veja que agora eu faço diferente a nossa lista de resultado a de cima List<SalesRecord>>. mas agora esta é uma lista de agrupamento por departamento, então <List<IGrouping<Department, SalesRecord>>> 
        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            //neste momento eu estou preparando o objeto para consulta como se fosse um (sql), mas não é. seria expresão lambda 
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            return await result
                .Include(x => x.Seller)
                //aqui estou fazendo um inner Join
                .Include(x => x.Seller.Department)
                //aqui estou Oredenando de oredem decrescente por data
                .OrderByDescending(x => x.Date)
                //agora vou agrupar por departamento
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();

            //depois de feito isto vamos no controllador (SalesRecordsController) fazer o Update
        }

    }
}
