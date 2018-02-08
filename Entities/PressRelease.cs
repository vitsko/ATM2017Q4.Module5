namespace Entities
{
    using System;
    using Utility;

    public class PressRelease
    {
        public PressRelease(int id)
        {
            this.Id = id;
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

        public string[] Title { get; private set; }

        public long SizeOfImageByAnnouncement { get; private set; }

        public string Announcement { get; private set; }

        public long SizeOfFileToWatchPDF { get; private set; }

        public long SizeOfFileToDownloadPDF { get; private set; }

        public DateTime Date { get; private set; }

        public int Id { get; private set; }

        public PressRelease WithTitle(string date, string name)
        {
            this.Title = new string[]
            {
                date,
                name
            };

            return this;
        }

        public PressRelease WithSizeOfFile(SizeOfFile fileType, string url)
        {
            switch (fileType)
            {
                case SizeOfFile.Image:
                    this.SizeOfImageByAnnouncement = Helper.GetContentLengthByLink(url);
                    break;

                case SizeOfFile.WatchPDF:
                    this.SizeOfFileToWatchPDF = Helper.GetContentLengthByLink(url);
                    break;

                case SizeOfFile.DownloadPDF:
                    this.SizeOfFileToDownloadPDF = Helper.GetContentLengthByLink(url);
                    break;
            }

            return this;
        }

        public PressRelease WithAnnouncement(string textOfAnnouncement)
        {
            this.Announcement = textOfAnnouncement;
            return this;
        }

        public PressRelease WithDate(DateTime date)
        {
            this.Date = date;
            return this;
        }
    }
}