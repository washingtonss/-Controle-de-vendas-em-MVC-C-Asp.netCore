using System;

namespace SalesWebMvc.Service.Exceptions
{
    //fazer a implementação basica de exceção personalizada
    public class IntegrityException : ApplicationException
    {
        //vou repassar a minha class IntegrityException para super classe  base(message)
        public IntegrityException(string message) : base(message)
        {

        }
    }
}
