using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class PrescriptionDto
{
    public int AppointmentId { get; set; }

    [Required]
    public string Medication { get; set; }

    [Required]
    public string Dosage { get; set; }

    [Required]
    public string Instruction { get; set; }
}