﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.Advert
{
    public class AdvertImage: File
    {
        public List<Advert> Advert { get; set; }
        public int? Index { get; set; }
    }
}
