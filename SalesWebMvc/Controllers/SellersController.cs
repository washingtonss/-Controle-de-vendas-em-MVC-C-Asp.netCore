
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;      
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Service;
using SalesWebMvc.Service.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        //inicio-injeção de dependencia 
        //  dependencia para acessar SellerService
        private readonly SellerService _sellerService;

        //vamos fazer mais uma dependencia para o  SellersControler. Não podemos esquecer de acrecenta-lo no construtor, logo abaixo.
        private readonly DepartmentService _departmentService;





        //e o contrutor injeta esta dependencia.
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;

        }
        //fim injeção de dependencia

        //este IActionResult retorna view um index, para mim.
        public async Task<IActionResult> Index()//IActionResult é o tipo de return sobre todas as ações.
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }


        // 1º Criei no Index.cshtml o link <a></a>
        //2º Criei a ação IActionResult Create() na minha pagina Controller SellersController.
        //3º Vou na View SubPasta Seller e crio View Create. Que vai exibir a ação. Logo eu desenho o html da View Create
        //vamos criar a ação do nosso botão
        public async Task<IActionResult> Create()
        {//este método create vai nos abrir o formulário para cadastrar o vendedor.

            // desta forma ele vai buscar no banco de dados todos os departamentos.
            var departments = await _departmentService.FindAllAsync();
            //var viewModel = new SellerFormViewModel { Departments = departments };
            var viewModel = new SellerFormViewModel { Departments = departments };

            //a minha tela de cadastro quando ela for acionada pela primeira vez ela vai receber a viewModel seria a tabela populada
            return View(viewModel);
        }

        [HttpPost]//colocando esta anotação. Desta forma estou dizendo que esta é uma ação de post e não de get. 
        [ValidateAntiForgeryToken]//esta anotação é para previnir que a minha aplicação sofra ataques CSRF. Este ataque e quando alguem aproveita a sua sessão para enviar ataques maliciosos
        public async Task<IActionResult> Create(Seller seller)
        {
            //este if é apenas a validação
            //enquanto o usuario não preencher o formulário isto fica acontecendo
            //este if está dizendo assim, se não  for valido retorna a mesma view com o objeto (seller), quer dizer fica na mesma pagina
            if (!ModelState.IsValid)
            {
                //nesta duas variaveis eu trago o departamento e o seller crio a viewModel e carrego tudo no retorno. Que vai devolver a lista
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

           await _sellerService.InsertAsync(seller);
            //neste RedirectToAction para redirecionar a minha pargina para o index depois da inserção eu coloquei (nameof(Index) este nameof servi para melhorar a manutenção mesmo que eu troque o nome da minha ação do index principal ele já troca automaticamente
            return RedirectToAction(nameof(Index));
        }
        //após termos implementado no SellerService public void Remove (int id). vamos agora implementar o Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não existi" });
            }
            //vamos buscar no banco de dados
            var obj =await _sellerService.FindByIdAsync(id.Value);
            //se este obj que eu busquei no bd for igual a nulo, significa que não existi então vai me retornar uma pagina de erro.
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não existi" });
            }

            //agora sim se tudo deu certo eu vou pedir para o meu (objeto) var obj que eu declarei a duas linhas atrás
            //me retorne View(obj); com o objeto deletado.

            return View(obj);
        }
        //criar uma outra ação delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try {
           await _sellerService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
            }
            //ao capturamos a msg do bd IntegrityException. Fazemos o direcionamento para pagina de erro... return RedirectToAction(nameof(Error), new { message = e.Message });
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        //agora no meu controlador vou criar a ação Detalhes
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não foi fornecido" });
            }
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não excontrado!" });
            }
            return View(obj);
        }
        //vamos criar a ação Edit. Lembrando esta ação de Edit recebe um id como argumento
        public async Task<IActionResult> Edit(int? id)
        {   // veja que já começo testando, se o id e igual nulo ou não existi
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não existi" });
            }
            //vou criar uma variavel para testar se existi este id no banco de dados
            var obj = await _sellerService.FindByIdAsync(id.Value);
            // se for igual a nullo siginifica que tambem não existi no banco de dados
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não econtrado" });
            }
            // testei se ele existi. depois testei se ele é nulo.
            //passando por tudo isto.
            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel
            { Seller = obj, Departments = departments };

            return View(viewModel);
        }

        //criar a ação edit para o método POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            //este if é apenas a validação
            //Lembrete fazndo desta forma ele faz funcionar as validações mesmo com o javascript desabilitado. Isto é muito bom
            //enquanto o usuario não preencher o formulário isto fica acontecendo
            //este if está dizendo assim, se não  for valido retorna a mesma view com o objeto (seller), quer dizer fica na mesma pagina
            if (!ModelState.IsValid)
            {
                //nesta duas variaveis eu trago o departamento e o seller crio a viewModel e carrego tudo no retorno. Ele retorna as validações
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não corresponde" });
            }
            try
            {
                //se passar pelo if significa que está ok. Ai eu chamo o Update atualizar
               await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }//estou colocando esta chamda dentro de um try cacth
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
           }
            //implementar ação error.
            //ação de erro não precisa ser assincrona por que ela não tem acesso a dados ela vai retornar a view 
            public IActionResult Error (string message)
            {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                //este é macete do framework para pegar o Id interno da requisição
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
            }
        }
    }
