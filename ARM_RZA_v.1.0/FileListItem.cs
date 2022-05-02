namespace ARM_RZA_v._1._0
{
    public class FileListItem
    {
        private string filePath;
        private string date;

        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
            }
        }
        public string Date
        {
            get { return date; }
            set
            {
                date = value;
            }
        }
    }
}
