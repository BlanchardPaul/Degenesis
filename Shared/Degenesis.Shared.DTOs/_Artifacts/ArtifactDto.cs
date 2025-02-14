using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Degenesis.Shared.DTOs.Artifacts;

public class ArtifactDto : ArtifactCreateDto
{
    public Guid Id { get; set; }
}