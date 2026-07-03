using Microsoft.AspNetCore.Mvc;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Repositories;
using sprint16_EF_LearningPlatform.API.DTOs;

namespace sprint16_EF_LearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController(AuthorsRepository authorsRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var authors = await authorsRepository.Get();
        return Ok(authors);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAuthorRequest request)
    {
        await authorsRepository.Add(request.id, request.userName);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await authorsRepository.Delete(id);
        return Ok();
    }
}