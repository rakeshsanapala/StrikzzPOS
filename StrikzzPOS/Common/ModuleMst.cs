﻿using System.ComponentModel.DataAnnotations;

namespace StrikzzPOS.Common
{
    public class ModuleMst
    {
        [Key]
        public int pk_moduleid { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDesc { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string AreaName { get; set; }
        public string ImgUrl { get; set; }
        public int IsActive { get; set; }
    }
}