using Fitness.Shared.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Application.Users.Queries
{
    public sealed record GetCurrentUserInfo : IRequest<ProfileDto?>;
}
