using Comments.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comments.Application.Repositories
{
    public interface ICommentRepository
    {
        IEnumerable[] GetComments();
        bool AddComment();
    }
}
