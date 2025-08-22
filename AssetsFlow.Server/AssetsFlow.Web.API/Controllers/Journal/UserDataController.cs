using Asp.Versioning;
using AssetsFlowWeb.Services.Models;
using AssetsFlowWeb.Services.Models.Journal;
using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Journal.Services;
using HsR.Web.API.Controllers;
using HsR.Web.API.Controllers.Journal;
using HsR.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssetsFlowWeb.API.Controllers.Journal;

[Route("hsr-api/v{version:apiVersion}/journal/userdata")]
[ApiVersion("1.0")]
[ApiController]
public class UserDataController : JournalControllerBase
{
    public UserDataController(
        IJournalRepositoryWrapper journalAccess,
        ILogger<JournalControllerBase> logger,
        IMapper mapper) : base(journalAccess, logger, mapper) { }

    [HttpGet]
    public async Task<ActionResult<UserDataDTO>> GetUserData()
    {
        var userId = GetUserIdFromClaims();
        var userData = await _journalAccess.UserDataRepo.GetOrCreateUserDataAsync(userId);


        var userDataDTO = _mapper.Map<UserDataDTO>(userData);
        userDataDTO.Symbols = await _journalAccess.UserDataRepo.GetAllAvailableSymbolsAsync(userId);

        return Ok(userDataDTO);
    }
}