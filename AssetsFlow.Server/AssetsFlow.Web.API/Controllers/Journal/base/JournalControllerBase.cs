using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Web.API.Controllers;
using HsR.Web.API.Services;
using HsR.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsR.Web.API.Controllers.Journal
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class JournalControllerBase : HsRControllerBase
    {
        private protected readonly IJournalRepositoryWrapper _journalAccess;
        private protected readonly IMapper _mapper;

        public JournalControllerBase(
            IJournalRepositoryWrapper journalAccess, 
            ILogger<JournalControllerBase> logger, 
            IMapper mapper) : base(logger) 
        {
            _journalAccess = journalAccess ?? throw new ArgumentNullException(nameof(journalAccess));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected Guid GetUserIdFromClaims()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "userId" || c.Type.EndsWith("nameidentifier"));
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("User ID not found in JWT claims.");
            return userId;
        }
    }
}
