using API_Folhas.Models;

namespace API_Folhas.Service
{
    public class FolhaPagamentoService
    {
        private readonly DataContext _context;
        
        public FolhaPagamentoService(DataContext context) => _context = context;

        public FolhaPagamento SalvarFolhaPagamento(FolhaPagamento folhaPagamento, Funcionario func)
        {
            double salarioBruto = CalculaSalarioBruto(folhaPagamento.QuantidadeHoras, folhaPagamento.ValorHora);
            double impostoRenda = ImpostoRenda(salarioBruto);
            double inss = Inss(salarioBruto);
            double fgts = Fgts(salarioBruto);

            folhaPagamento.SalarioBruto = salarioBruto;
            folhaPagamento.ImpostoRenda = impostoRenda;
            folhaPagamento.ImpostoInss = inss;
            folhaPagamento.ImpostoFgts = fgts;
            folhaPagamento.SalarioLiquido = (salarioBruto - impostoRenda - inss);
            folhaPagamento.Funcionario = func;
            
            _context.Folhas.Add(folhaPagamento);
            _context.SaveChanges();

            return folhaPagamento;
        }

        private double CalculaSalarioBruto(double horas, double valorHora)
        {
            return horas * valorHora;
        }

        private double ImpostoRenda(double salarioBruto)
        {
            if (salarioBruto < 1903.99)
            {
                return 0.0;
            }
            else if(salarioBruto < 2826.66)
            {
                return (salarioBruto * 0.075) - 142.80;
            }
            else if(salarioBruto < 3751.06)
            {
                return (salarioBruto * 0.15) - 354.80;
            }
            else if(salarioBruto <= 4664.68)
            {
                return (salarioBruto * 0.225) - 636.13;
            }
            else
            {
                return (salarioBruto * 0.275) - 869.36;
            }
        }
        
        private double Inss(double salarioBruto)
        {
            if (salarioBruto < 1693.73)
            {
                return salarioBruto * 0.8;
            }
            else if(salarioBruto < 2822.91)
            {
                return salarioBruto * 0.9;
            }
            else if(salarioBruto <= 5645.80)
            {
                return salarioBruto * 0.11;
            }
            else
            {
                return 621.03;
            }
        }

        private double Fgts(double salarioBruto)
        {
            return salarioBruto * 0.08;
        }
    }
}