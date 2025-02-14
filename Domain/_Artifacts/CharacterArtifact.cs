using Domain.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Artifacts;

public class CharacterArtifact
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid ArtifactId { get; set; }
    public Artifact Artifact { get; set; } = new();
    public int ChargeInMagazine { get; set; }
}