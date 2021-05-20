using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.ResponseModels
{
    public class ResponseFlashcardCommentsModel
    {
        public int CommentId { get; set; }
        public string Comment { get; set; }
    }
}
