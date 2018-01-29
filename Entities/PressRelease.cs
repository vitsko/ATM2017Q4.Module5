namespace Entities
{
    using System;

    public class PressRelease
    {
        public PressRelease()
        {
            this.SizeOfFileToDownloadPDF = -1;
            this.SizeOfFileToWatchPDF = -1;
            this.SizeOfImageByAnnouncement = -1;
        }

        public enum SizeOfFile
        {
            Image,
            WatchPDF,
            DownloadPDF
        }

        public string[] Title { get; set; }

        public long SizeOfImageByAnnouncement { get; set; }

        public string Announcement { get; set; }

        public long SizeOfFileToWatchPDF { get; set; }

        public long SizeOfFileToDownloadPDF { get; set; }

        public DateTime Date { get; set; }

        public int Id { get; set; }
    }
}