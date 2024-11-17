using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Shared.DTOs.Logs
{
    public sealed record UserLog
    {
        public int Id { get; init; } // Уникальный идентификатор лога
        public int UserId { get; init; } // Идентификатор пользователя
        public string Action { get; init; } // Действие, выполненное пользователем
        public DateTime Timestamp { get; init; } // Время выполнения действия
        public string Details { get; init; } // Дополнительные детали
    }
}
