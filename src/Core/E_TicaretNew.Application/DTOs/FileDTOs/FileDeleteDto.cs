using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_TicaretNew.Application.DTOs.FileDTOs;

public class FileDeleteDto
{
    [Required]
    public string FilePath { get; set; }
}
