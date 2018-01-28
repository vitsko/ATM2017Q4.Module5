namespace TestPressReleases.Entities
{
    using System;

    internal class PressRelease
    {
        internal PressRelease()
        {
            this.SizeOfFileToDownloadPDF = -1;
            this.SizeOfFileToWatchPDF = -1;
            this.SizeOfImageByAnnouncement = -1;
        }

        internal enum SizeOfFile
        {
            Image,
            WatchPDF,
            DownloadPDF
        }

        internal string[] Title { get; set; }

        internal long SizeOfImageByAnnouncement { get; set; }

        internal string Announcement { get; set; }

        internal long SizeOfFileToWatchPDF { get; set; }

        internal long SizeOfFileToDownloadPDF { get; set; }

        internal DateTime Date { get; set; }

        internal int Id { get; set; }
    }
}