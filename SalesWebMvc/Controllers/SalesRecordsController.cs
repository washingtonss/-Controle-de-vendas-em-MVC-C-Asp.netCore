
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Service;
using System;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    //este nome surgiu pelo nome da classe de dominio (Models SalesRecord) mas agora no plural SalesRecordsController
    //Já na view o nome da pasta será SalesRecords. Pode procurar que tem..
    public class SalesRecordsController : Controller
    {
        //vamos inserir injeção de dependencia SalesRecordService, para pode usa-lá
        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            //se não possuir um valor minimo
            if (!minDate.HasValue)
            {
                //se a caso não possuir o ano eu que que me de como padrão o ano atual (DateTime.Now.Year, 1, 1) dia seja 01/Janeiro. Isto faz com que se o usuario não informar o ano. o sistema pesquise pelo inicio do ano atual
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            //fazer a mesma coisa mas agora com a data maixma. se a data maxima não for passada pelo cliente
            if (!maxDate.HasValue)
            {
                //o sistema vai recebero ano atual
                maxDate = DateTime.Now;
            }
            //agora eu passara estas datas ou inoformações para minha view. olha que importante. Lembrando que vou acrescentar lá view SimpleSearch o value=@ViewData["minDate"]
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            //estou trazendo _salesRecordService.FindByDate. lá da Classe SalesRecordService. Por isto precisei da injeção de dependencia
            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        //para fazer o grouping(Grupo) 1º eu fui no SalesRecordService. 2ºfui no controlador (SalesRecordsController) e em 3º por ultimo vou View HTML (GoupingSearch) 
        //fazendo por grupo, estou vindo do (SalesRecordService)
        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            //estes dois if é caso a data não seja informada ele pesquisará pela da atual
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }


            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
            return View(result);

        }
    }
}