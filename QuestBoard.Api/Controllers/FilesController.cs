using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestBoard.Application.Abstractions;
using QuestBoard.Application.DTOs.Common;
using QuestBoard.Domain.Entities;

namespace QuestBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly IFileStorage _fileStorage;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FilesController(IFileStorage fileStorage, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _fileStorage = fileStorage;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost]
    [RequestSizeLimit(20_000_000)]
    public async Task<ActionResult<FileDto>> Upload([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        await using var stream = file.OpenReadStream();
        var saveResult = await _fileStorage.SaveAsync(stream, file.FileName, file.ContentType, cancellationToken);

        var resource = new FileResource
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            Length = file.Length,
            Path = saveResult.Path,
            UploadedById = GetUserId(),
        };

        await _unitOfWork.Files.AddAsync(resource, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<FileDto>(resource);
        return CreatedAtAction(nameof(GetById), new { id = resource.Id }, dto);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FileDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var resource = await _unitOfWork.Files.GetByIdAsync(id, cancellationToken);
        if (resource is null)
        {
            return NotFound();
        }

        var dto = _mapper.Map<FileDto>(resource);
        return Ok(dto);
    }

    private Guid GetUserId()
    {
        var idClaim = User.FindFirst("sub") ?? throw new InvalidOperationException("Missing subject claim.");
        return Guid.Parse(idClaim.Value);
    }
}
