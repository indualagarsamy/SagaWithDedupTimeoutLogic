using System;
using NServiceBus.Saga;

namespace TestSaga
{
    public class OrderSaga : Saga<OrderSagaData>,
                          IAmStartedByMessages<StartSaga>, IHandleTimeouts<MyCustomTimeout>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<StartSaga>(message => message.WorkflowId)
                    .ToSaga(sagaData => sagaData.WorkflowId);
        }

        public void Handle(StartSaga message)
        {
            Data.WorkflowId = message.WorkflowId;
            RequestTimeout(TimeSpan.FromSeconds(5), new MyCustomTimeout() {TimeoutCounter = 1});
        }

        public void Timeout(MyCustomTimeout state)
        {
            Console.WriteLine("Timeout being processed is:" + state.TimeoutCounter);
            Console.WriteLine("SagaData LastProcessedTimeout is:" + Data.LastProcessedTimeout);

            if (state.TimeoutCounter <= Data.LastProcessedTimeout)
            {
                Console.WriteLine("This is a duplicate timeout - not going to process");
                return;
            }
            if (Data.LastProcessedTimeout == 10)
            {
                Console.WriteLine("Marking saga as complete");
                MarkAsComplete();
            }
            Data.LastProcessedTimeout = state.TimeoutCounter;
            RequestTimeout(TimeSpan.FromSeconds(2), new MyCustomTimeout() { TimeoutCounter = state.TimeoutCounter + 1});
        }
    }

    public class MyCustomTimeout
    {
        public int TimeoutCounter { get; set; }
    }
}
