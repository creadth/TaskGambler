﻿using InoGambling.Data.Models;

namespace InoGambling.Core.Services.Projects.Models
{
    public class CreateProjectResult
    {
        public CreateProjectState State { get; set; }
        public Project Project { get; set; }
    }

    public enum CreateProjectState : byte
    {
        Ok = 0,
        ProjectExists = 1,
    }
}
