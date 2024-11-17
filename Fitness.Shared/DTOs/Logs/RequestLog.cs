using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Shared.DTOs.Logs
{
    // DTO для Reports_Logs
    // DTO для Requests_Log
    public sealed record RequestLog
    {
        public int Id { get; init; } // Уникальный идентификатор лога
        public int RequestId { get; init; } // Идентификатор запроса
        public string Action { get; init; } // Действие, выполненное с запросом
        public DateTime Timestamp { get; init; } // Время выполнения действия
        public string Details { get; init; } // Дополнительные детали
    }
}
