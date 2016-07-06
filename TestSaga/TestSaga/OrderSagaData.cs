using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using NServiceBus.Saga;

namespace TestSaga
{
    public class OrderSagaData : ContainSagaData
    {
     
        [Unique]
        public virtual Guid WorkflowId { get; set; }

        public virtual int LastProcessedTimeout { get; set; }
        
    }
}