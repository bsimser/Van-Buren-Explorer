﻿using System;
using System.IO;
using System.Text;
using VanBurenExplorerLib.Models;

namespace VanBurenExplorerLib.Specs
{
    public class TextFileSpecification : IFileSpecification
    {
        public bool IsSatisfiedBy(FileProperties properties)
        {
            return GetEncoding(properties.FullPath) != null;
        }

        /// <summary>
        /// adapted from https://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static Encoding GetEncoding(string filename)
        {
            var encodingByBOM = GetEncodingByBOM(filename);
            if (encodingByBOM != null)
                return encodingByBOM;

            // BOM not found :(, so try to parse characters into several encodings
            var encodingByParsingUTF8 = GetEncodingByParsing(filename, Encoding.UTF8);
            if (encodingByParsingUTF8 != null)
                return encodingByParsingUTF8;

            // NOTE: we skip iso-8859-1 here because some binary files will return this and we're only interested in text files

            var encodingByParsingUTF7 = GetEncodingByParsing(filename, Encoding.UTF7);
            if (encodingByParsingUTF7 != null)
                return encodingByParsingUTF7;

            // no encoding or cannot determine it
            return null;
        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM)  
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        private static Encoding GetEncodingByBOM(string filename)
        {
            // Read the BOM
            var byteOrderMark = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(byteOrderMark, 0, 4);
            }

            // Analyze the BOM
            if (byteOrderMark[0] == 0x2b && byteOrderMark[1] == 0x2f && byteOrderMark[2] == 0x76) return Encoding.UTF7;
            if (byteOrderMark[0] == 0xef && byteOrderMark[1] == 0xbb && byteOrderMark[2] == 0xbf) return Encoding.UTF8;
            if (byteOrderMark[0] == 0xff && byteOrderMark[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (byteOrderMark[0] == 0xfe && byteOrderMark[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (byteOrderMark[0] == 0 && byteOrderMark[1] == 0 && byteOrderMark[2] == 0xfe && byteOrderMark[3] == 0xff) return Encoding.UTF32;

            return null;    // no BOM found
        }

        private static Encoding GetEncodingByParsing(string filename, Encoding encoding)
        {
            var encodingVerifier = Encoding.GetEncoding(encoding.BodyName, new EncoderExceptionFallback(), new DecoderExceptionFallback());

            try
            {
                using (var textReader = new StreamReader(filename, encodingVerifier, detectEncodingFromByteOrderMarks: true))
                {
                    while (!textReader.EndOfStream)
                    {
                        textReader.ReadLine(); // in order to increment the stream position
                    }

                    // all text parsed ok
                    return textReader.CurrentEncoding;
                }
            }
            catch (Exception)
            {
                // don't care about the exception as we're only looking for a valid encoding
            }

            return null; 
        }


    }
}