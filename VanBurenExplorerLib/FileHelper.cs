namespace VanBurenExplorerLib
{
    public static class FileHelper
    {
        /// <summary>
        /// Get human readable size
        /// https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net
        /// TODO maybe use humanizr or something similar instead of writing code for this?
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetFormattedSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            var order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return $"{len:0} {sizes[order]}";
        }
    }
}