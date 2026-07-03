using Microsoft.AspNetCore.Mvc;
using sprint16_EF_LearningPlatform.API.DTOs;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Repositories;

namespace sprint16_EF_LearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController(LessonsRepository lessonsRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateLesson([FromBody] CreateLessonRequest request)
    {
        await lessonsRepository.Add(request.id, request.courseId, request.title, request.description, request.lessonText);
        return Ok();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var lesson = await lessonsRepository.GetById(id);
        return Ok(lesson);
    }

    [HttpGet("course/{courseId:guid}")]
    public async Task<IActionResult> GetByCourseId(Guid courseId)
    {
        var lessons = await lessonsRepository.GetByCourseId(courseId);
        return Ok(lessons);   
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await lessonsRepository.Delete(id);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLessonRequest request)
    {
        await lessonsRepository.Update(id, request.title, request.description, request.lessonText);
        return NoContent();
    }
}