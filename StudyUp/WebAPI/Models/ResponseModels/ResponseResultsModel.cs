using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.ResponseModels
{
    public class ResponseResultModel
    {
        public string Username { get; set; }
        public double Score { get; set; }
    }

    public class ResponseResultsModel
    {
        public List<ResponseResultModel> List { get; set; }
    }
}
