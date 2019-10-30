using System;
using System.Collections.Generic;
using System.Text;

namespace Solar.Services
{
    public interface IResizeImageService
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
    }
}
