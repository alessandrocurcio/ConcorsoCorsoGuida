using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcorsoCorsoGuidaEntities.Contracts
{
    public class ApiResponse<T>
    {
        public T Result { get; set; }
        public bool HasError { get; set; }
        public string Message { get; set; }

        public ApiResponse(T Result){
            this.Result = Result;
        }
        public ApiResponse(Exception ex)
        {
            this.Message = ex.Message;
            this.HasError = true;
        }
    }
}
