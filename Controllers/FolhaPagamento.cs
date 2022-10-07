using System.Linq;
using API_Folhas.Models;
using API_Folhas.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Folhas.Controllers
{
    
    [ApiController]
    [Route("api/folhas")]
    public class FolhaPagamentoController : ControllerBase
    {
        private readonly FolhaPagamentoService service;
        private readonly DataContext _context;

        public FolhaPagamentoController(DataContext context)
        {
            _context = context;
            service = new FolhaPagamentoService(context);
        }
        
        [Route("cadastrar")]
        [HttpPost]
        public IActionResult Cadastrar([FromBody] FolhaPagamento folha)
        {
            var func = _context.Funcionarios.Find(folha.FuncionarioId);
            return func == null ?  NotFound("Nenhum usuário com o esse id: " + folha.FuncionarioId) : Created("", service.SalvarFolhaPagamento(folha, func));
        }
        
        [Route("listar")]
        [HttpGet]
        public IActionResult Listar() => Ok(_context.Folhas.Include(pagamento => pagamento.Funcionario).ToList());
        
        [Route("buscar/{cpf}/{mes}/{ano}")]
        [HttpGet]
        public IActionResult GetByCpMesAno([FromRoute] string cpf, [FromRoute] int mes, [FromRoute] int ano)
        {
            FolhaPagamento folha = _context.Folhas.
                Include(pagamento => pagamento.Funcionario).
                FirstOrDefault(pagamento =>
                pagamento.Funcionario.Cpf.Equals(cpf) && 
                pagamento.CriadoEm.Month.Equals(mes) &&
                pagamento.CriadoEm.Year.Equals(ano));

            return folha == null ? NotFound("Nenhuma folha encontrada!") : Ok(folha);
        }
        
        [Route("filtrar/{mes}/{ano}")]
        [HttpGet]
        public IActionResult GetByCpMesAno([FromRoute] int mes, [FromRoute] int ano)
        {
            return Ok(_context.Folhas.Include(pagamento => pagamento.Funcionario).ToList().FindAll(pagamento =>
                pagamento.CriadoEm.Month.Equals(mes) &&
                pagamento.CriadoEm.Year.Equals(ano)));
        }
    }
}