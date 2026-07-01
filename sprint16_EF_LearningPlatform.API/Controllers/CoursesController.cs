using Microsoft.AspNetCore.Mvc;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Repositories;
using sprint16_EF_LearningPlatform.API.DTOs;

namespace sprint16_EF_LearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(CoursesRepository coursesRepository) :ControllerBase
{
    private readonly CoursesRepository _coursesRepository = coursesRepository;
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var courses = await _coursesRepository.Get();
        return Ok(courses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
    {
        await _coursesRepository.Add(request.id,request.authorId,request.title,request.description,request.price);
        return Ok();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var course = await _coursesRepository.GetById(id);
        return Ok(course);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _coursesRepository.Delete(id);
        return Ok();
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourseRequest request)
    {
        await _coursesRepository.Update(id,request.authorId, request.title, request.description, request.price);
        return NoContent();
    }
}