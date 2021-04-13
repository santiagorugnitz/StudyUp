using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface
{
    public interface IGroupLogic
    {
        Group AddGroup(Group group, string creatorsToken);
    }
}
