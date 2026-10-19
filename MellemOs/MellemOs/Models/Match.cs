using System;
using System.Collections.Generic;
using System.Text;

namespace MellemOs.Models;

public class Match
{
    public int Id { get; set; }

    public Match(int id)
    {
        Id = id;
    }
    List<Player> Players { get; set; } = new List<Player>();
}
