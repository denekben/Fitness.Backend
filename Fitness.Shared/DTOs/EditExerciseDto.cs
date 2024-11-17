using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Shared.DTOs
{
    public sealed record EditExerciseDto(
        int? RepeatQuantity,
        int? SetQuantity,
        string? Name
    );
}
