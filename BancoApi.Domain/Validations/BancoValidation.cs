//using FluentValidation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BancoApi.Domain.Validations
//{
//    public class BancoValidation : AbstractValidator<Banco>
//    {
//        public BancoValidation()
//        {
//            RuleFor(b => b.Nome).NotEmpty().MinimumLength(2).MaximumLength(10);
//            RuleFor(d => d.DataNascimento).NotEmpty().LessThan(DateTime.Now).GreaterThan(DateTime.Now.AddYears(-130));
//            RuleFor(model => model.Id).LessThanOrEqualTo(100);
//        }


//    }
//}
