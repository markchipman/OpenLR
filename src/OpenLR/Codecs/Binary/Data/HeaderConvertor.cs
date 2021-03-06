﻿// The MIT License (MIT)

// Copyright (c) 2016 Ben Abelshausen

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace OpenLR.Codecs.Binary.Data
{
    /// <summary>
    /// Represents a header convertor that encodes/decodes header information from/to the binary OpenLR format.
    /// </summary>
    public static class HeaderConvertor
    {
        /// <summary>
        /// Decodes a byte from the given data array into header information.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static Header Decode(byte[] data, int startIndex)
        {
            if (startIndex > data.Length) { throw new ArgumentOutOfRangeException("startIndex"); }

            return HeaderConvertor.Decode(data[startIndex]);
        }

        /// <summary>
        /// Decodes the given byte into header information.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Header Decode(byte data)
        {
            return new Header()
            {
                ArF1 = (data & (1 << 6)) != 0,
                IsPoint = (data & (1 << 5)) != 0,
                ArF0 = (data & (1 << 4)) != 0,
                HasAttributes = (data & (1 << 3)) != 0,
                Version = HeaderConvertor.DecodeVersion(data)
            };
        }

        /// <summary>
        /// Decodes the version information from the binary header.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static ushort DecodeVersion(byte data)
        {            
            // create mask.
            int mask = 7;
            int value = (data & mask);
            return (ushort)value;
        }

        /// <summary>
        /// Encodes an header 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="header"></param>
        public static void Encode(byte[] data, int startIndex, Header header)
        {
            if (startIndex > data.Length) { throw new ArgumentOutOfRangeException("startIndex"); }

            var headerByte = (byte)header.Version;
            headerByte = (byte)(headerByte + (header.HasAttributes ? 8 : 0));
            headerByte = (byte)(headerByte + (header.ArF0 ? 16 : 0));
            headerByte = (byte)(headerByte + (header.IsPoint ? 32 : 0));
            headerByte = (byte)(headerByte + (header.ArF1 ? 64 : 0));

            data[startIndex] = headerByte;
        }
    }

    /// <summary>
    /// Represents an OpenLR binary header.
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Gets or sets ArF1 status bit.
        /// </summary>
        public bool ArF1 { get; set; }

        /// <summary>
        /// Gets or sets the IsPoint status bit.
        /// </summary>
        public bool IsPoint { get; set; }

        /// <summary>
        /// Gets or sets ArF0 status bit.
        /// </summary>
        public bool ArF0 { get; set; }

        /// <summary>
        /// Gets or sets the has attributes status bit.
        /// </summary>
        public bool HasAttributes { get; set; }

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        public ushort Version { get; set; }
    }
}