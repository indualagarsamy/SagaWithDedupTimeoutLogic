using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace TestSaga
{
    public class StartSaga : ICommand
    {
        public Guid WorkflowId { get; set; }
    }
}
