﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace 倒计时.Models
{
    public class ImageCropperConfig
    {
        public StorageFile ImageFile { get; set; }
        public double AspectRatio { get; set; } = -1;
        public bool CircularCrop { get; set; }
    }
}
