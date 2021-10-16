using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.Validation
{
    public class BancoValidation : AbstractValidator<Banco>
    {
        public BancoValidation()
        {
            RuleFor(b => b.Nome).NotEmpty().WithMessage("Nome não pode estar em branco").WithErrorCode("2526");
            RuleFor(d => d.DataNascimento).NotEmpty().WithMessage("Erro na data de nascimento").LessThan(DateTime.Now).GreaterThan(DateTime.Now.AddYears(-130)).WithErrorCode("3310");
           // RuleFor(model => model.Id).LessThanOrEqualTo(100).WithErrorCode("3031");
        }
    }
}
