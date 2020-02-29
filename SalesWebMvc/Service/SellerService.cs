
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using SalesWebMvc.Service.Exceptions;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;


namespace SalesWebMvc.Service
{
    public class SellerService
    {
        //vou declara uma dependencia para classe SalesWebMvcContext e o nome dela será _context
        //este readonly é para previnir que esta dependencia não possa ser alterada.
        private readonly SalesWebMvcContext _context;
        

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        //vamos  a nossa operação finAll (para retornar toda a lista que houver no bd)
        public async Task<List<Seller>> FindAllAsync()
        {
            //implementação em uma linha para retornar todos os meus dados do bd
            return await _context.Seller.ToListAsync(); //ele vai acessar a minha fonte de dados da tabela de vendores, e vai me retornar convertendo o ToList (a minha lista)

        }

        //vamos criar um método para inserir no banco de dados 
        public async Task InsertAsync(Seller obj)
        {
            //obj.Department = _context.Department.First(); //ao menos um departamento esta sendo colocado.
            _context.Add(obj); //para inserir no bd
           await _context.SaveChangesAsync(); //e para confirmar a inserção ou alteração.
        }
        //vamos deletar o vendedor (Seller)
        //este FindById vai receber um int id e vai retornar para gente o vendedor (Seller que possui este id)
        //Se o vendedor não existir eu retorno nullo.
        public async Task<Seller> FindByIdAsync(int id)
        {
            //vou dar um FirstOrDefault no meu obj (objeto Id) esperando que seja igual ao id do delete, se for vai deletar.
            //lembrando que com FirstOrDefault estou usando o link
            //este Include(obj => obj.Department) está fazendo o Join entre as tabelas
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }
        //tudo fica limpo e claro usando link acima. Muito bem agora vamos implementar o método remove
        //Remove recebendo um id 
        public async Task RemoveAsync(int id)
        {
            //o try cacth entra para capturar  a excessão DbUpdateException vinda do bd
            // ai pegamos ela e passamos esta a nivel de serviço throw new IntegrityException(e.Message);
            try
            { 
            //quando eu consigo pegar este obj(objeto) no var. Na linha a baixo eu peço para remover
            var obj = await _context.Seller.FindAsync(id);
            //peço para remover, do dbset, mas para remover geral do banco de dado ai vem na linha abaixo SaveChanges()
            _context.Seller.Remove(obj);
            //agora sim vai remover do banco de dados.
           await _context.SaveChangesAsync();
            }
            //veja vamos capturar esta excessão e  colocar uma nova em nivel de serviço
            catch (DbUpdateException e)
            {
                throw new IntegrityException("Não posso deletar o vendedor por que ele ou ela tem vendas! ");
            }
        }

        //vamos atualizar agora
        //vou pegar obj e testar se o id já existi. Como eu estou atulizando ele precisa existir.
        public async Task UpdateAsync (Seller obj)
        {
            //hasAny estou perguntando tem algum
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            //se não existir um id igual, eu vou lançar uma excessão
           if (!hasAny)
            {
                throw new DllNotFoundException("Id not found");
            }
            try { //vou colocar um tratamento de erro try catch, para tentar pegar erro de concorrencia que ocorra no bd
            //se passar por este if significa que existi o id dai vamos atualiza-lo.
            _context.Update(obj);
            //completar atualizando no banco de dados.
            await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                //se isto ocorrer eu relanço uma outra exceção em nivel de serviço
                throw new DbConcurrencyException(e.Message);

               }
            }
        }
       
    }

