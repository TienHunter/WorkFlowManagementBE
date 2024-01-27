using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Utils
{
    public static class TaskCreator
    {
        public static void CreateTaskAsync(Func<Task> callback)
        {
            // Tạo và chạy task
            Task.Run(async () =>
            {
                try
                {
                    // Gọi callback và đợi kết thúc của nó
                    await callback().ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // Task bị hủy
                    Console.WriteLine("Task cancelled");
                }
            });
        }

    }
}
