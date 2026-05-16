using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class DepartmentDto
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}