using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Shared.DTOs.Logs
{
    // DTO для Reports_Logs
    public sealed record ReportLog
    {
        public int Id { get; init; } // Уникальный идентификатор лога
        public int ReportId { get; init; } // Идентификатор отчета
        public string Action { get; init; } // Действие, выполненное с отчетом
        public DateTime Timestamp { get; init; } // Время выполнения действия
        public string Details { get; init; } // Дополнительные детали
    }
}
